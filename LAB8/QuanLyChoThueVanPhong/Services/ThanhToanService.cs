using System;
using System.Data;
using QuanLyChoThueVanPhong.Data;

namespace QuanLyChoThueVanPhong.Services
{
    public static class ThanhToanService
    {
        // Thu tien thue hang thang (tu ky 7): thu ky ke tiep, chi khi ky do da bat dau va con trong thoi han hop dong.
        public static KetQuaXuLy ThuTienThueThang(string soHD, string nguoiThu, DateTime ngayThu)
        {
            if (string.IsNullOrWhiteSpace(soHD)) return KetQuaXuLy.Fail("Chon hop dong");
            if (string.IsNullOrWhiteSpace(nguoiThu)) return KetQuaXuLy.Fail("Nhap nguoi thu");
            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        DataTable hd = Db.QueryInTransaction(cn, tran,
                            "SELECT NgayBatDau, TrangThai FROM HopDongThue WITH (UPDLOCK) WHERE SoHD=@hd", Db.P("@hd", soHD));
                        if (hd.Rows.Count == 0 || hd.Rows[0]["TrangThai"].ToString() != QuyDinh.HDHieuLuc)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Hop dong khong ton tai hoac da thanh ly"); }

                        var tt = HopDongService.TinhTrang(cn, tran, soHD, ngayThu);
                        int ky = tt.DaTraDenKy + 1;
                        if (ky <= QuyDinh.SoThangTraTruoc)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Hop dong chua thu tien tra truoc 6 thang dau"); }
                        if (ky > tt.TongSoKy)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Da thu du tien thue den het thoi han hop dong"); }
                        DateTime dauKy = ((DateTime)hd.Rows[0]["NgayBatDau"]).AddMonths(ky - 1);
                        if (dauKy > ngayThu.Date)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Chua den ky " + ky + " (bat dau " + dauKy.ToString("dd/MM/yyyy") + "), tien thue tra vao dau moi thang"); }

                        decimal soTien = HopDongService.TienThueKy(cn, tran, soHD, ky);
                        if (soTien <= 0) { tran.Rollback(); return KetQuaXuLy.Fail("Ky " + ky + " khong con phong nao dang thue"); }
                        string soPhieu = QuyDinh.SinhMa("PTT");
                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO PhieuThuTienThue(SoPhieu, SoHD, TuKy, DenKy, SoTien, NgayThu, LoaiThu, NguoiThu)
                              VALUES(@sp,@hd,@k,@k,@st,@n,@l,@nt)",
                            Db.P("@sp", soPhieu), Db.P("@hd", soHD), Db.P("@k", ky), Db.P("@st", soTien),
                            Db.P("@n", ngayThu.Date), Db.P("@l", QuyDinh.HangThang), Db.P("@nt", nguoiThu));
                        tran.Commit();
                        return KetQuaXuLy.Ok("Thu tien thue ky " + ky + " (thang " + dauKy.ToString("MM/yyyy") + "): "
                                             + soTien.ToString("N0") + " d. Phieu: " + soPhieu, soPhieu);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi thu tien thue: " + ex.Message);
                    }
                }
            }
        }

        public static string MoTaTinhTrang(string soHD)
        {
            var tt = HopDongService.TinhTrang(soHD, DateTime.Today);
            if (tt == null) return "";
            return "Ky hien tai: " + tt.KyHienTai + "/" + tt.TongSoKy + "   Da thu den ky: " + tt.DaTraDenKy
                   + (tt.ConNo ? "   CON NO " + tt.SoKyNo + " KY" : "   Khong no tien thue");
        }

        public static DataTable DsPhieuThu(string soHD)
            => Db.Query(@"SELECT SoPhieu, TuKy, DenKy, SoTien, NgayThu, LoaiThu, NguoiThu
                          FROM PhieuThuTienThue WHERE SoHD=@hd ORDER BY TuKy", Db.P("@hd", soHD));
    }
}
