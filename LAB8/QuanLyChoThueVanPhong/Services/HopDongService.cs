using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using QuanLyChoThueVanPhong.Data;

namespace QuanLyChoThueVanPhong.Services
{
    public static class HopDongService
    {
        // Lap hop dong thue nhieu van phong. Trong 1 transaction:
        // hop dong + chi tiet tung phong + cap nhat phong "Dang thue" + phieu thu tra truoc 6 thang dau.
        public static KetQuaXuLy LapHopDong(string maKH, DateTime ngayBatDau, int soThang, List<string> dsPhong,
            string noiDung, string nguoiThu)
        {
            if (string.IsNullOrWhiteSpace(maKH)) return KetQuaXuLy.Fail("Chon khach hang");
            if (soThang < QuyDinh.SoThangToiThieu) return KetQuaXuLy.Fail("Thoi gian thue it nhat " + QuyDinh.SoThangToiThieu + " thang");
            if (dsPhong == null || dsPhong.Count == 0) return KetQuaXuLy.Fail("Hop dong phai co it nhat 1 van phong");
            if (dsPhong.Distinct().Count() != dsPhong.Count) return KetQuaXuLy.Fail("Danh sach phong bi trung");
            if (ngayBatDau.Date < DateTime.Today) return KetQuaXuLy.Fail("Ngay bat dau thue khong duoc truoc ngay ky");
            if (string.IsNullOrWhiteSpace(nguoiThu)) return KetQuaXuLy.Fail("Nhap nguoi thu tien");

            DateTime ngayKetThuc = ngayBatDau.Date.AddMonths(soThang);
            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        decimal tongGiaThang = 0;
                        var gia = new Dictionary<string, decimal>();
                        foreach (string ma in dsPhong)
                        {
                            DataTable p = Db.QueryInTransaction(cn, tran,
                                "SELECT GiaThue, TrangThai FROM Phong WITH (UPDLOCK) WHERE MaPhong=@m", Db.P("@m", ma));
                            if (p.Rows.Count == 0) { tran.Rollback(); return KetQuaXuLy.Fail("Phong " + ma + " khong ton tai"); }
                            if (p.Rows[0]["TrangThai"].ToString() != QuyDinh.PhongTrong)
                            { tran.Rollback(); return KetQuaXuLy.Fail("Phong " + ma + " khong con trong"); }
                            gia[ma] = (decimal)p.Rows[0]["GiaThue"];
                            tongGiaThang += gia[ma];
                        }

                        string soHD = QuyDinh.SinhMa("HD");
                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO HopDongThue(SoHD, MaKH, NgayKy, NgayBatDau, SoThang, NgayKetThuc, NoiDung, TrangThai)
                              VALUES(@hd,@kh,@nk,@bd,@st,@kt,@nd,@tt)",
                            Db.P("@hd", soHD), Db.P("@kh", maKH), Db.P("@nk", DateTime.Today), Db.P("@bd", ngayBatDau.Date),
                            Db.P("@st", soThang), Db.P("@kt", ngayKetThuc), Db.P("@nd", noiDung), Db.P("@tt", QuyDinh.HDHieuLuc));

                        foreach (string ma in dsPhong)
                        {
                            Db.ExecuteInTransaction(cn, tran,
                                @"INSERT INTO ChiTietHopDong(SoHD, MaPhong, GiaThue, NgayBatDau, NgayKetThuc)
                                  VALUES(@hd,@p,@g,@bd,@kt)",
                                Db.P("@hd", soHD), Db.P("@p", ma), Db.P("@g", gia[ma]), Db.P("@bd", ngayBatDau.Date), Db.P("@kt", ngayKetThuc));
                            Db.ExecuteInTransaction(cn, tran, "UPDATE Phong SET TrangThai=@tt WHERE MaPhong=@p",
                                Db.P("@tt", QuyDinh.PhongDangThue), Db.P("@p", ma));
                        }

                        decimal traTruoc = tongGiaThang * QuyDinh.SoThangTraTruoc;
                        string soPhieu = QuyDinh.SinhMa("PTT");
                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO PhieuThuTienThue(SoPhieu, SoHD, TuKy, DenKy, SoTien, NgayThu, LoaiThu, NguoiThu)
                              VALUES(@sp,@hd,1,@den,@st,@n,@l,@nt)",
                            Db.P("@sp", soPhieu), Db.P("@hd", soHD), Db.P("@den", QuyDinh.SoThangTraTruoc), Db.P("@st", traTruoc),
                            Db.P("@n", DateTime.Today), Db.P("@l", QuyDinh.TraTruoc), Db.P("@nt", nguoiThu));

                        tran.Commit();
                        return KetQuaXuLy.Ok("Lap hop dong thanh cong. So HD: " + soHD + ". Da thu truoc 6 thang: "
                                             + traTruoc.ToString("N0") + " d (phieu " + soPhieu + ")", soHD);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi lap hop dong: " + ex.Message);
                    }
                }
            }
        }

        // ---- Tinh trang thanh toan tien thue (dung chung cho thanh toan, gia han, thanh ly) ----
        public static TinhTrangThue TinhTrang(SqlConnection cn, SqlTransaction tran, string soHD, DateTime ngay)
        {
            DataTable hd = Db.QueryInTransaction(cn, tran, "SELECT NgayBatDau, NgayKetThuc FROM HopDongThue WHERE SoHD=@hd", Db.P("@hd", soHD));
            if (hd.Rows.Count == 0) return null;
            DateTime bd = (DateTime)hd.Rows[0]["NgayBatDau"], kt = (DateTime)hd.Rows[0]["NgayKetThuc"];
            object daTra = Db.ScalarInTransaction(cn, tran, "SELECT ISNULL(MAX(DenKy), 0) FROM PhieuThuTienThue WHERE SoHD=@hd", Db.P("@hd", soHD));
            return new TinhTrangThue
            {
                KyHienTai = ngay.Date < bd ? 0 : QuyDinh.SoThangGiua(bd, ngay.Date) + 1,
                TongSoKy = QuyDinh.SoThangGiua(bd, kt),
                DaTraDenKy = Convert.ToInt32(daTra)
            };
        }

        public static TinhTrangThue TinhTrang(string soHD, DateTime ngay)
        {
            using (var cn = Db.GetConnection())
            {
                cn.Open();
                return TinhTrang(cn, null, soHD, ngay);
            }
        }

        // Tien thue cua ky k = tong gia cac phong con hieu luc vao ngay dau ky do (phong co the da duoc gia han rieng).
        public static decimal TienThueKy(SqlConnection cn, SqlTransaction tran, string soHD, int ky)
        {
            object bd = Db.ScalarInTransaction(cn, tran, "SELECT NgayBatDau FROM HopDongThue WHERE SoHD=@hd", Db.P("@hd", soHD));
            DateTime dauKy = ((DateTime)bd).AddMonths(ky - 1);
            object tong = Db.ScalarInTransaction(cn, tran,
                @"SELECT ISNULL(SUM(GiaThue), 0) FROM ChiTietHopDong
                  WHERE SoHD=@hd AND NgayBatDau <= @d AND NgayKetThuc > @d", Db.P("@hd", soHD), Db.P("@d", dauKy));
            return (decimal)tong;
        }

        // Thanh ly hop dong: khong con no tien thue, hoa don dich vu, phieu phat; tra phong ve "Trong".
        public static KetQuaXuLy ThanhLyHopDong(string soHD)
        {
            if (string.IsNullOrWhiteSpace(soHD)) return KetQuaXuLy.Fail("Chon hop dong");
            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        object tt = Db.ScalarInTransaction(cn, tran, "SELECT TrangThai FROM HopDongThue WHERE SoHD=@hd", Db.P("@hd", soHD));
                        if (tt == null || tt.ToString() != QuyDinh.HDHieuLuc) { tran.Rollback(); return KetQuaXuLy.Fail("Hop dong khong con hieu luc"); }
                        var ttThue = TinhTrang(cn, tran, soHD, DateTime.Today);
                        if (ttThue.ConNo) { tran.Rollback(); return KetQuaXuLy.Fail("Hop dong con no " + ttThue.SoKyNo + " ky tien thue"); }
                        object noDV = Db.ScalarInTransaction(cn, tran, "SELECT COUNT(*) FROM HoaDonDichVu WHERE SoHD=@hd AND TrangThai=@c",
                            Db.P("@hd", soHD), Db.P("@c", QuyDinh.ChuaThanhToan));
                        if (Convert.ToInt32(noDV) > 0) { tran.Rollback(); return KetQuaXuLy.Fail("Con hoa don dich vu chua thanh toan"); }
                        object noPhat = Db.ScalarInTransaction(cn, tran, "SELECT COUNT(*) FROM PhieuPhat WHERE SoHD=@hd AND TrangThai=@c",
                            Db.P("@hd", soHD), Db.P("@c", QuyDinh.ChuaNop));
                        if (Convert.ToInt32(noPhat) > 0) { tran.Rollback(); return KetQuaXuLy.Fail("Con phieu phat chua nop"); }

                        Db.ExecuteInTransaction(cn, tran,
                            "UPDATE Phong SET TrangThai=@trong WHERE MaPhong IN (SELECT MaPhong FROM ChiTietHopDong WHERE SoHD=@hd)",
                            Db.P("@trong", QuyDinh.PhongTrong), Db.P("@hd", soHD));
                        Db.ExecuteInTransaction(cn, tran, "UPDATE HopDongThue SET TrangThai=@tt WHERE SoHD=@hd",
                            Db.P("@tt", QuyDinh.HDDaThanhLy), Db.P("@hd", soHD));
                        tran.Commit();
                        return KetQuaXuLy.Ok("Thanh ly hop dong " + soHD + " thanh cong, cac phong da tra ve trang thai trong");
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi thanh ly: " + ex.Message);
                    }
                }
            }
        }

        public static DataTable DsHopDong(bool chiHieuLuc = false)
            => Db.Query(@"SELECT hd.SoHD, hd.MaKH, kh.TenCongTy, hd.NgayKy, hd.NgayBatDau, hd.SoThang, hd.NgayKetThuc, hd.TrangThai,
                                 hd.SoHD + ' - ' + kh.TenCongTy AS HienThi
                          FROM HopDongThue hd JOIN KhachHang kh ON hd.MaKH = kh.MaKH
                          WHERE @chi = 0 OR hd.TrangThai = @tt
                          ORDER BY hd.NgayKy DESC", Db.P("@chi", chiHieuLuc ? 1 : 0), Db.P("@tt", QuyDinh.HDHieuLuc));

        public static DataTable ChiTietHopDong(string soHD)
            => Db.Query(@"SELECT ct.MaPhong, t.TenTang, p.DienTich, ct.GiaThue, ct.NgayBatDau, ct.NgayKetThuc
                          FROM ChiTietHopDong ct JOIN Phong p ON ct.MaPhong = p.MaPhong JOIN Tang t ON p.MaTang = t.MaTang
                          WHERE ct.SoHD=@hd ORDER BY ct.MaPhong", Db.P("@hd", soHD));
    }
}
