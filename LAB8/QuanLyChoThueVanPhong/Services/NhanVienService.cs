using System;
using System.Data;
using System.Linq;
using QuanLyChoThueVanPhong.Data;

namespace QuanLyChoThueVanPhong.Services
{
    public static class NhanVienService
    {
        // Cong ty bao cao nhan vien moi: phong lam viec phai thuoc hop dong dang hieu luc cua chinh cong ty do.
        public static KetQuaXuLy BaoCaoNhanVienMoi(NhanVienMoi nv)
        {
            if (nv == null || string.IsNullOrWhiteSpace(nv.HoTen)) return KetQuaXuLy.Fail("Nhap ho ten nhan vien");
            if (string.IsNullOrWhiteSpace(nv.CCCD) || nv.CCCD.Trim().Length != 12 || !nv.CCCD.Trim().All(char.IsDigit))
                return KetQuaXuLy.Fail("So CCCD phai gom 12 chu so");
            if (string.IsNullOrWhiteSpace(nv.MaKH) || string.IsNullOrWhiteSpace(nv.MaPhong)) return KetQuaXuLy.Fail("Chon cong ty va phong lam viec");
            if (string.IsNullOrWhiteSpace(nv.ChucVu)) return KetQuaXuLy.Fail("Nhap chuc vu");

            object thue = Db.Scalar(@"SELECT COUNT(*) FROM ChiTietHopDong ct JOIN HopDongThue hd ON ct.SoHD = hd.SoHD
                                      WHERE hd.MaKH=@kh AND ct.MaPhong=@p AND hd.TrangThai=@tt AND ct.NgayKetThuc > @n",
                Db.P("@kh", nv.MaKH), Db.P("@p", nv.MaPhong), Db.P("@tt", QuyDinh.HDHieuLuc), Db.P("@n", DateTime.Today));
            if (Convert.ToInt32(thue) == 0) return KetQuaXuLy.Fail("Phong " + nv.MaPhong + " khong thuoc hop dong dang hieu luc cua cong ty nay");
            object trung = Db.Scalar("SELECT COUNT(*) FROM NhanVienCongTy WHERE CCCD=@c", Db.P("@c", nv.CCCD.Trim()));
            if (Convert.ToInt32(trung) > 0) return KetQuaXuLy.Fail("So CCCD nay da duoc bao cao");

            string ma = QuyDinh.SinhMa("NV");
            try
            {
                Db.Execute(@"INSERT INTO NhanVienCongTy(MaNV, HoTen, CCCD, HinhAnh, MaKH, ChucVu, MaPhong, DienThoai, NgayBaoCao, TrangThai)
                             VALUES(@m,@ht,@c,@ha,@kh,@cv,@p,@dt,@n,@tt)",
                    Db.P("@m", ma), Db.P("@ht", nv.HoTen.Trim()), Db.P("@c", nv.CCCD.Trim()), Db.P("@ha", nv.HinhAnh),
                    Db.P("@kh", nv.MaKH), Db.P("@cv", nv.ChucVu.Trim()), Db.P("@p", nv.MaPhong), Db.P("@dt", nv.DienThoai),
                    Db.P("@n", DateTime.Today), Db.P("@tt", QuyDinh.DangLam));
            }
            catch (Exception ex) { return KetQuaXuLy.Fail("Loi khi luu nhan vien: " + ex.Message); }
            return KetQuaXuLy.Ok("Da ghi nhan nhan vien moi. Ma NV: " + ma, ma);
        }

        // Nhan vien nghi viec: the giu xe dang dung bi huy theo.
        public static KetQuaXuLy NghiViec(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return KetQuaXuLy.Fail("Chon nhan vien");
            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    int n = Db.ExecuteInTransaction(cn, tran, "UPDATE NhanVienCongTy SET TrangThai=@nghi WHERE MaNV=@m AND TrangThai=@lam",
                        Db.P("@nghi", QuyDinh.DaNghi), Db.P("@m", maNV), Db.P("@lam", QuyDinh.DangLam));
                    if (n == 0) { tran.Rollback(); return KetQuaXuLy.Fail("Nhan vien khong ton tai hoac da nghi"); }
                    Db.ExecuteInTransaction(cn, tran, "UPDATE TheGiuXe SET TrangThai=@huy WHERE MaNV=@m AND TrangThai=@dung",
                        Db.P("@huy", QuyDinh.TheDaHuy), Db.P("@m", maNV), Db.P("@dung", QuyDinh.TheDangDung));
                    tran.Commit();
                    return KetQuaXuLy.Ok("Da cap nhat nhan vien " + maNV + " nghi viec, huy the giu xe (neu co)");
                }
            }
        }

        // Cap the giu xe: nhan vien dang lam, chua co the dang dung; bien so bat buoc tru xe dap, khong trung.
        public static KetQuaXuLy CapTheGiuXe(string maNV, string loaiXe, string bienSo, string hieuXe, string mauXe)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return KetQuaXuLy.Fail("Chon nhan vien");
            if (!QuyDinh.LoaiXe.Contains(loaiXe)) return KetQuaXuLy.Fail("Loai xe khong hop le");
            if (loaiXe != "Xe dap" && string.IsNullOrWhiteSpace(bienSo)) return KetQuaXuLy.Fail("Nhap bien so xe");
            object tt = Db.Scalar("SELECT TrangThai FROM NhanVienCongTy WHERE MaNV=@m", Db.P("@m", maNV));
            if (tt == null || tt.ToString() != QuyDinh.DangLam) return KetQuaXuLy.Fail("Nhan vien khong con lam viec tai cao oc");
            object coThe = Db.Scalar("SELECT COUNT(*) FROM TheGiuXe WHERE MaNV=@m AND TrangThai=@d", Db.P("@m", maNV), Db.P("@d", QuyDinh.TheDangDung));
            if (Convert.ToInt32(coThe) > 0) return KetQuaXuLy.Fail("Nhan vien da co the giu xe dang dung");
            string bs = string.IsNullOrWhiteSpace(bienSo) ? null : bienSo.Trim().ToUpper();
            if (bs != null)
            {
                object trung = Db.Scalar("SELECT COUNT(*) FROM TheGiuXe WHERE BienSo=@b AND TrangThai=@d", Db.P("@b", bs), Db.P("@d", QuyDinh.TheDangDung));
                if (Convert.ToInt32(trung) > 0) return KetQuaXuLy.Fail("Bien so " + bs + " da duoc cap the");
            }
            string ma = QuyDinh.SinhMa("TX");
            try
            {
                Db.Execute(@"INSERT INTO TheGiuXe(MaThe, MaNV, LoaiXe, BienSo, HieuXe, MauXe, NgayCap, TrangThai)
                             VALUES(@m,@nv,@l,@b,@h,@mau,@n,@tt)",
                    Db.P("@m", ma), Db.P("@nv", maNV), Db.P("@l", loaiXe), Db.P("@b", bs), Db.P("@h", hieuXe), Db.P("@mau", mauXe),
                    Db.P("@n", DateTime.Today), Db.P("@tt", QuyDinh.TheDangDung));
            }
            catch (Exception ex) { return KetQuaXuLy.Fail("Loi khi cap the: " + ex.Message); }
            return KetQuaXuLy.Ok("Cap the giu xe thanh cong. Ma the: " + ma, ma);
        }

        public static KetQuaXuLy HuyThe(string maThe)
        {
            if (string.IsNullOrWhiteSpace(maThe)) return KetQuaXuLy.Fail("Chon the");
            int n = Db.Execute("UPDATE TheGiuXe SET TrangThai=@huy WHERE MaThe=@m AND TrangThai=@d",
                Db.P("@huy", QuyDinh.TheDaHuy), Db.P("@m", maThe), Db.P("@d", QuyDinh.TheDangDung));
            return n > 0 ? KetQuaXuLy.Ok("Da huy the " + maThe) : KetQuaXuLy.Fail("The khong ton tai hoac da huy");
        }

        public static DataTable DsNhanVien()
            => Db.Query(@"SELECT nv.MaNV, nv.HoTen, nv.CCCD, kh.TenCongTy, nv.ChucVu, nv.MaPhong, t.TenTang,
                                 nv.DienThoai, nv.NgayBaoCao, nv.TrangThai, nv.HinhAnh,
                                 nv.MaNV + ' - ' + nv.HoTen AS HienThi
                          FROM NhanVienCongTy nv
                          JOIN KhachHang kh ON nv.MaKH = kh.MaKH
                          JOIN Phong p ON nv.MaPhong = p.MaPhong JOIN Tang t ON p.MaTang = t.MaTang
                          ORDER BY nv.MaNV");

        public static DataTable DsPhongDangThueCuaKhach(string maKH)
            => Db.Query(@"SELECT ct.MaPhong FROM ChiTietHopDong ct JOIN HopDongThue hd ON ct.SoHD = hd.SoHD
                          WHERE hd.MaKH=@kh AND hd.TrangThai=@tt AND ct.NgayKetThuc > @n ORDER BY ct.MaPhong",
                Db.P("@kh", maKH), Db.P("@tt", QuyDinh.HDHieuLuc), Db.P("@n", DateTime.Today));

        public static DataTable DsTheGiuXe()
            => Db.Query(@"SELECT tx.MaThe, tx.MaNV, nv.HoTen, kh.TenCongTy, nv.MaPhong, tx.LoaiXe, tx.BienSo, tx.HieuXe, tx.MauXe,
                                 tx.NgayCap, tx.TrangThai
                          FROM TheGiuXe tx JOIN NhanVienCongTy nv ON tx.MaNV = nv.MaNV JOIN KhachHang kh ON nv.MaKH = kh.MaKH
                          ORDER BY tx.NgayCap DESC");
    }
}
