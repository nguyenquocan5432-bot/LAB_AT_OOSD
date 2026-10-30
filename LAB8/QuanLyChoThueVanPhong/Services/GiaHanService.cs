using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyChoThueVanPhong.Data;

namespace QuanLyChoThueVanPhong.Services
{
    public static class GiaHanService
    {
        // Dieu kien gia han: hop dong con hieu luc, khong no tien thue mat bang, da thanh toan du hoa don dich vu.
        public static KetQuaXuLy KiemTraDieuKien(SqlConnection cn, SqlTransaction tran, string soHD)
        {
            object tt = Db.ScalarInTransaction(cn, tran, "SELECT TrangThai FROM HopDongThue WHERE SoHD=@hd", Db.P("@hd", soHD));
            if (tt == null) return KetQuaXuLy.Fail("Hop dong khong ton tai");
            if (tt.ToString() != QuyDinh.HDHieuLuc) return KetQuaXuLy.Fail("Hop dong da thanh ly");
            var ttThue = HopDongService.TinhTrang(cn, tran, soHD, DateTime.Today);
            if (ttThue.ConNo) return KetQuaXuLy.Fail("Khong duoc gia han: con no " + ttThue.SoKyNo + " ky tien thue mat bang");
            object noDV = Db.ScalarInTransaction(cn, tran, "SELECT COUNT(*) FROM HoaDonDichVu WHERE SoHD=@hd AND TrangThai=@c",
                Db.P("@hd", soHD), Db.P("@c", QuyDinh.ChuaThanhToan));
            if (Convert.ToInt32(noDV) > 0) return KetQuaXuLy.Fail("Khong duoc gia han: con " + noDV + " hoa don dich vu chua thanh toan");
            return KetQuaXuLy.Ok("Du dieu kien gia han");
        }

        public static KetQuaXuLy KiemTraDieuKien(string soHD)
        {
            using (var cn = Db.GetConnection())
            {
                cn.Open();
                return KiemTraDieuKien(cn, null, soHD);
            }
        }

        // Lap phieu gia han cho mot phong: so thang <= 1/2 thoi gian thue; cap nhat ngay het han phong va hop dong (1 transaction).
        public static KetQuaXuLy LapPhieuGiaHan(string soHD, string maPhong, int soThangGiaHan, string nguoiLap)
        {
            if (string.IsNullOrWhiteSpace(soHD) || string.IsNullOrWhiteSpace(maPhong)) return KetQuaXuLy.Fail("Chon hop dong va phong");
            if (soThangGiaHan <= 0) return KetQuaXuLy.Fail("So thang gia han phai lon hon 0");
            if (string.IsNullOrWhiteSpace(nguoiLap)) return KetQuaXuLy.Fail("Nhap nguoi lap phieu");

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        var dk = KiemTraDieuKien(cn, tran, soHD);
                        if (!dk.ThanhCong) { tran.Rollback(); return dk; }

                        object soThangThue = Db.ScalarInTransaction(cn, tran, "SELECT SoThang FROM HopDongThue WHERE SoHD=@hd", Db.P("@hd", soHD));
                        int toiDa = Convert.ToInt32(soThangThue) / 2;
                        if (soThangGiaHan > toiDa)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Thoi gian gia han toi da " + toiDa + " thang (1/2 thoi gian thue " + soThangThue + " thang)"); }

                        object hetHan = Db.ScalarInTransaction(cn, tran,
                            "SELECT NgayKetThuc FROM ChiTietHopDong WITH (UPDLOCK) WHERE SoHD=@hd AND MaPhong=@p",
                            Db.P("@hd", soHD), Db.P("@p", maPhong));
                        if (hetHan == null) { tran.Rollback(); return KetQuaXuLy.Fail("Phong " + maPhong + " khong thuoc hop dong " + soHD); }
                        DateTime cu = (DateTime)hetHan;
                        if (cu <= DateTime.Today) { tran.Rollback(); return KetQuaXuLy.Fail("Phong " + maPhong + " da het han thue, khong the gia han"); }
                        DateTime moi = cu.AddMonths(soThangGiaHan);

                        string soPhieu = QuyDinh.SinhMa("PGH");
                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO PhieuGiaHan(SoPhieu, SoHD, MaPhong, NgayLap, SoThangGiaHan, NgayHetHanCu, NgayHetHanMoi, NguoiLap)
                              VALUES(@sp,@hd,@p,@nl,@st,@cu,@moi,@ng)",
                            Db.P("@sp", soPhieu), Db.P("@hd", soHD), Db.P("@p", maPhong), Db.P("@nl", DateTime.Today),
                            Db.P("@st", soThangGiaHan), Db.P("@cu", cu), Db.P("@moi", moi), Db.P("@ng", nguoiLap));
                        Db.ExecuteInTransaction(cn, tran, "UPDATE ChiTietHopDong SET NgayKetThuc=@moi WHERE SoHD=@hd AND MaPhong=@p",
                            Db.P("@moi", moi), Db.P("@hd", soHD), Db.P("@p", maPhong));
                        Db.ExecuteInTransaction(cn, tran,
                            "UPDATE HopDongThue SET NgayKetThuc = (SELECT MAX(NgayKetThuc) FROM ChiTietHopDong WHERE SoHD=@hd) WHERE SoHD=@hd",
                            Db.P("@hd", soHD));
                        tran.Commit();
                        return KetQuaXuLy.Ok("Lap phieu gia han thanh cong. So phieu: " + soPhieu + ". Phong " + maPhong
                                             + " thue den " + moi.ToString("dd/MM/yyyy"), soPhieu);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi lap phieu gia han: " + ex.Message);
                    }
                }
            }
        }

        public static DataTable DsPhieuGiaHan(string soHD)
            => Db.Query(@"SELECT SoPhieu, MaPhong, NgayLap, SoThangGiaHan, NgayHetHanCu, NgayHetHanMoi, NguoiLap
                          FROM PhieuGiaHan WHERE SoHD=@hd ORDER BY NgayLap DESC, SoPhieu DESC", Db.P("@hd", soHD));
    }
}
