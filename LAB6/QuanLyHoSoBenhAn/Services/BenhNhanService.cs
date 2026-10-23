using System;
using System.Data;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Services
{
    public static class BenhNhanService
    {
        // Dang ky benh nhan moi + dong le phi nam + cap ho so, tat ca trong 1 transaction
        public static KetQuaXuLy DangKy(string maBN, string hoTen, string diaChi, DateTime ngaySinh,
            string dienThoai, string anhChupPath, decimal soTienLePhi, out string maHoSoMoi)
        {
            maHoSoMoi = null;

            if (string.IsNullOrWhiteSpace(maBN) || string.IsNullOrWhiteSpace(hoTen))
                return KetQuaXuLy.Fail("Ma benh nhan va ho ten khong duoc de trong");
            if (string.IsNullOrWhiteSpace(dienThoai))
                return KetQuaXuLy.Fail("Dien thoai khong duoc de trong");
            if (soTienLePhi < 0)
                return KetQuaXuLy.Fail("So tien le phi khong hop le");

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        object trungSdt = Db.ScalarInTransaction(cn, tran,
                            "SELECT COUNT(*) FROM BenhNhan WHERE DienThoai=@d", Db.P("@d", dienThoai));
                        if (Convert.ToInt32(trungSdt) > 0)
                        { tran.Rollback(); return KetQuaXuLy.Fail("So dien thoai nay da duoc dang ky truoc do"); }

                        object trungMa = Db.ScalarInTransaction(cn, tran,
                            "SELECT COUNT(*) FROM BenhNhan WHERE MaBN=@m", Db.P("@m", maBN));
                        if (Convert.ToInt32(trungMa) > 0)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Ma benh nhan da ton tai"); }

                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO BenhNhan(MaBN, HoTen, DiaChi, NgaySinh, DienThoai, AnhChupPath, NgayDangKy)
                              VALUES(@m,@h,@d,@n,@dt,@a,@nd)",
                            Db.P("@m", maBN), Db.P("@h", hoTen), Db.P("@d", diaChi), Db.P("@n", ngaySinh),
                            Db.P("@dt", dienThoai), Db.P("@a", anhChupPath), Db.P("@nd", DateTime.Today));

                        string maLePhi = "LP" + DateTime.Now.ToString("yyMMddHHmmss");
                        Db.ExecuteInTransaction(cn, tran,
                            "INSERT INTO LePhiNam(MaLePhi, MaBN, NamApDung, NgayDong, SoTien) VALUES(@lp,@m,@n,@nd,@s)",
                            Db.P("@lp", maLePhi), Db.P("@m", maBN), Db.P("@n", DateTime.Today.Year),
                            Db.P("@nd", DateTime.Today), Db.P("@s", soTienLePhi));

                        string maHoSo = "HS" + DateTime.Now.ToString("yyMMddHHmmss");
                        Db.ExecuteInTransaction(cn, tran,
                            "INSERT INTO HoSoBenhAn(MaHoSo, MaBN, NgayCap, TrangThai) VALUES(@h,@m,@nd,@tt)",
                            Db.P("@h", maHoSo), Db.P("@m", maBN), Db.P("@nd", DateTime.Today), Db.P("@tt", QuyDinh.HoSoHoatDong));

                        tran.Commit();
                        maHoSoMoi = maHoSo;
                        return KetQuaXuLy.Ok("Dang ky va cap ho so benh an thanh cong. Ma ho so: " + maHoSo, maHoSo);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi dang ky: " + ex.Message);
                    }
                }
            }
        }

        // Dong le phi cho nam moi (benh nhan da co ho so tu truoc)
        public static KetQuaXuLy DongLePhi(string maBN, int nam, decimal soTien)
        {
            if (Db.Scalar("SELECT COUNT(*) FROM LePhiNam WHERE MaBN=@m AND NamApDung=@n",
                Db.P("@m", maBN), Db.P("@n", nam)) is int c && c > 0)
                return KetQuaXuLy.Fail("Benh nhan da dong le phi cho nam nay roi");

            string maLePhi = "LP" + DateTime.Now.ToString("yyMMddHHmmss");
            Db.Execute("INSERT INTO LePhiNam(MaLePhi, MaBN, NamApDung, NgayDong, SoTien) VALUES(@lp,@m,@n,@nd,@s)",
                Db.P("@lp", maLePhi), Db.P("@m", maBN), Db.P("@n", nam), Db.P("@nd", DateTime.Today), Db.P("@s", soTien));
            return KetQuaXuLy.Ok("Dong le phi thanh cong");
        }

        public static DataTable TraCuuTheoMaHoSo(string maHoSo)
            => Db.Query(@"SELECT hs.MaHoSo, hs.TrangThai, bn.MaBN, bn.HoTen, bn.DiaChi, bn.NgaySinh, bn.DienThoai, bn.AnhChupPath
                          FROM HoSoBenhAn hs JOIN BenhNhan bn ON hs.MaBN = bn.MaBN
                          WHERE hs.MaHoSo = @m", Db.P("@m", maHoSo));

        public static DataTable TraCuuTheoTenHoacSdt(string tuKhoa)
            => Db.Query(@"SELECT hs.MaHoSo, hs.TrangThai, bn.MaBN, bn.HoTen, bn.DiaChi, bn.NgaySinh, bn.DienThoai
                          FROM HoSoBenhAn hs JOIN BenhNhan bn ON hs.MaBN = bn.MaBN
                          WHERE bn.HoTen LIKE @t OR bn.DienThoai LIKE @t",
                Db.P("@t", "%" + tuKhoa + "%"));

        public static DataTable LichSuDieuTri(string maHoSo)
            => Db.Query(@"SELECT pk.MaPhieuKham, pk.NgayGioKham, bs.HoTen AS BacSi, k.TenKhoa, pk.LoaiKham, pk.KetLuan, pk.TrangThai
                          FROM PhieuKhamBenh pk
                          JOIN BacSi bs ON pk.MaBacSi = bs.MaBacSi
                          JOIN Khoa k ON pk.MaKhoa = k.MaKhoa
                          WHERE pk.MaHoSo = @m ORDER BY pk.NgayGioKham DESC",
                Db.P("@m", maHoSo));
    }
}
