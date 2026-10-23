using System;
using System.Data;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Services
{
    public static class PhieuKhamService
    {
        // Lap phieu kham benh kem trieu chung dau tien, trong 1 transaction
        public static KetQuaXuLy LapPhieuKham(string maHoSo, string maBacSi, string maKhoa, string loaiKham,
            string tenTrieuChung, decimal? thanNhiet, string huyetAp, string moTaThem, out string maPhieuKhamMoi)
        {
            maPhieuKhamMoi = null;

            if (string.IsNullOrWhiteSpace(maHoSo)) return KetQuaXuLy.Fail("Chua chon ho so benh an");
            if (string.IsNullOrWhiteSpace(tenTrieuChung)) return KetQuaXuLy.Fail("Can ghi nhan it nhat 1 trieu chung");

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        object trangThaiHoSo = Db.ScalarInTransaction(cn, tran,
                            "SELECT TrangThai FROM HoSoBenhAn WHERE MaHoSo=@m", Db.P("@m", maHoSo));
                        if (trangThaiHoSo == null)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Ho so benh an khong ton tai"); }
                        if (trangThaiHoSo.ToString() != QuyDinh.HoSoHoatDong)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Ho so benh an da ngung hoat dong"); }

                        string maPhieuKham = "PK" + DateTime.Now.ToString("yyMMddHHmmss");
                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO PhieuKhamBenh(MaPhieuKham, MaHoSo, NgayGioKham, MaBacSi, MaKhoa, LoaiKham, TrangThai)
                              VALUES(@p,@h,@n,@b,@k,@l,@tt)",
                            Db.P("@p", maPhieuKham), Db.P("@h", maHoSo), Db.P("@n", DateTime.Now),
                            Db.P("@b", maBacSi), Db.P("@k", maKhoa), Db.P("@l", loaiKham), Db.P("@tt", QuyDinh.DangKham));

                        string maTrieuChung = "TC" + maPhieuKham;
                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO TrieuChung(MaTrieuChung, MaPhieuKham, TenTrieuChung, ThanNhiet, HuyetAp, MoTaThem)
                              VALUES(@tc,@p,@t,@th,@hu,@m)",
                            Db.P("@tc", maTrieuChung), Db.P("@p", maPhieuKham), Db.P("@t", tenTrieuChung),
                            Db.P("@th", thanNhiet), Db.P("@hu", huyetAp), Db.P("@m", moTaThem));

                        tran.Commit();
                        maPhieuKhamMoi = maPhieuKham;
                        return KetQuaXuLy.Ok("Lap phieu kham thanh cong. Ma phieu: " + maPhieuKham, maPhieuKham);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi lap phieu kham: " + ex.Message);
                    }
                }
            }
        }

        public static DataTable DsPhieuKham()
            => Db.Query(@"SELECT pk.MaPhieuKham, pk.MaHoSo, bn.HoTen, pk.NgayGioKham, bs.HoTen AS BacSi,
                                 k.TenKhoa, pk.LoaiKham, pk.TrangThai
                          FROM PhieuKhamBenh pk
                          JOIN HoSoBenhAn hs ON pk.MaHoSo = hs.MaHoSo
                          JOIN BenhNhan bn ON hs.MaBN = bn.MaBN
                          JOIN BacSi bs ON pk.MaBacSi = bs.MaBacSi
                          JOIN Khoa k ON pk.MaKhoa = k.MaKhoa
                          ORDER BY pk.NgayGioKham DESC");

        public static DataTable DsTrieuChung(string maPhieuKham)
            => Db.Query("SELECT TenTrieuChung, ThanNhiet, HuyetAp, MoTaThem FROM TrieuChung WHERE MaPhieuKham=@m",
                Db.P("@m", maPhieuKham));

        // ===== Xet nghiem =====
        public static KetQuaXuLy ChiDinhXetNghiem(string maPhieuKham, string loaiXetNghiem)
        {
            if (string.IsNullOrWhiteSpace(maPhieuKham)) return KetQuaXuLy.Fail("Chua chon phieu kham");
            if (string.IsNullOrWhiteSpace(loaiXetNghiem)) return KetQuaXuLy.Fail("Chua nhap loai xet nghiem");

            string maXN = "XN" + DateTime.Now.ToString("yyMMddHHmmss");
            Db.Execute("INSERT INTO XetNghiem(MaXetNghiem, MaPhieuKham, LoaiXetNghiem) VALUES(@x,@p,@l)",
                Db.P("@x", maXN), Db.P("@p", maPhieuKham), Db.P("@l", loaiXetNghiem));
            return KetQuaXuLy.Ok("Chi dinh xet nghiem thanh cong");
        }

        public static DataTable DsXetNghiem(string maPhieuKham)
            => Db.Query("SELECT MaXetNghiem, LoaiXetNghiem, KetQua, NgayCoKetQua FROM XetNghiem WHERE MaPhieuKham=@m",
                Db.P("@m", maPhieuKham));

        public static KetQuaXuLy CapNhatKetQuaXetNghiem(string maXetNghiem, string ketQua)
        {
            int n = Db.Execute("UPDATE XetNghiem SET KetQua=@k, NgayCoKetQua=@n WHERE MaXetNghiem=@m",
                Db.P("@k", ketQua), Db.P("@n", DateTime.Now), Db.P("@m", maXetNghiem));
            if (n == 0) return KetQuaXuLy.Fail("Khong tim thay xet nghiem");
            return KetQuaXuLy.Ok("Cap nhat ket qua xet nghiem thanh cong");
        }

        public static KetQuaXuLy GhiKetLuan(string maPhieuKham, string ketLuan)
        {
            int n = Db.Execute("UPDATE PhieuKhamBenh SET KetLuan=@k WHERE MaPhieuKham=@m",
                Db.P("@k", ketLuan), Db.P("@m", maPhieuKham));
            if (n == 0) return KetQuaXuLy.Fail("Khong tim thay phieu kham");
            return KetQuaXuLy.Ok("Ghi ket luan thanh cong");
        }
    }
}
