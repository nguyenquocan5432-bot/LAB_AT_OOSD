using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using QuanLyNhanSu.Data;

namespace QuanLyNhanSu.Services
{
    public static class ChamCongService
    {
        // Truong phong lap ke hoach phan cong ca lam viec cho 1 tuan (tu ngay -> tu ngay + 6).
        public static KetQuaXuLy LapKeHoachPhanCong(string maPB, DateTime tuNgay, string nguoiLap, List<ChiTietPhanCongItem> ds)
        {
            if (string.IsNullOrWhiteSpace(maPB)) return KetQuaXuLy.Fail("Chon phong ban");
            if (string.IsNullOrWhiteSpace(nguoiLap)) return KetQuaXuLy.Fail("Nhap nguoi lap ke hoach");
            if (ds == null || ds.Count == 0) return KetQuaXuLy.Fail("Ke hoach phai co it nhat 1 dong phan cong");
            DateTime denNgay = tuNgay.Date.AddDays(6);
            if (ds.Any(x => x.NgayLam.Date < tuNgay.Date || x.NgayLam.Date > denNgay))
                return KetQuaXuLy.Fail("Ngay lam phai nam trong tuan " + tuNgay.ToString("dd/MM") + " - " + denNgay.ToString("dd/MM"));
            if (ds.GroupBy(x => new { x.MaNV, Ngay = x.NgayLam.Date, x.MaCa }).Any(g => g.Count() > 1))
                return KetQuaXuLy.Fail("Co dong phan cong bi trung (nhan vien - ngay - ca)");

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        string maKH = QuyDinh.SinhMa("KH");
                        Db.ExecuteInTransaction(cn, tran,
                            "INSERT INTO KeHoachPhanCong(MaKH, MaPB, TuNgay, DenNgay, NguoiLap) VALUES(@k,@pb,@tu,@den,@nl)",
                            Db.P("@k", maKH), Db.P("@pb", maPB), Db.P("@tu", tuNgay.Date), Db.P("@den", denNgay), Db.P("@nl", nguoiLap));
                        foreach (var ct in ds)
                        {
                            Db.ExecuteInTransaction(cn, tran,
                                "INSERT INTO ChiTietPhanCong(MaKH, MaNV, NgayLam, MaCa) VALUES(@k,@nv,@n,@ca)",
                                Db.P("@k", maKH), Db.P("@nv", ct.MaNV), Db.P("@n", ct.NgayLam.Date), Db.P("@ca", ct.MaCa));
                        }
                        tran.Commit();
                        return KetQuaXuLy.Ok("Lap ke hoach phan cong thanh cong. Ma KH: " + maKH, maKH);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi lap ke hoach (co the nhan vien da duoc phan cong ca nay): " + ex.Message);
                    }
                }
            }
        }

        public static DataTable DsPhanCong(string maPB)
            => Db.Query(@"SELECT kh.MaKH, kh.TuNgay, kh.DenNgay, ct.MaNV, nv.HoTen, ct.NgayLam, ca.TenCa
                          FROM KeHoachPhanCong kh
                          JOIN ChiTietPhanCong ct ON kh.MaKH = ct.MaKH
                          JOIN NhanVien nv ON ct.MaNV = nv.MaNV
                          JOIN CaLamViec ca ON ct.MaCa = ca.MaCa
                          WHERE kh.MaPB = @pb
                          ORDER BY kh.TuNgay DESC, ct.NgayLam, ct.MaNV", Db.P("@pb", maPB));

        // Ban cham cong nhap bang cham cong thang; neu da co thi cap nhat (truoc khi tinh luong).
        public static KetQuaXuLy LuuChamCong(string maNV, int thang, int nam, decimal soNgayCong, decimal nghiPhep, decimal nghiKP)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return KetQuaXuLy.Fail("Chon nhan vien");
            if (thang < 1 || thang > 12 || nam < 2000) return KetQuaXuLy.Fail("Thang/nam khong hop le");
            int soNgayTrongThang = DateTime.DaysInMonth(nam, thang);
            if (soNgayCong < 0 || nghiPhep < 0 || nghiKP < 0) return KetQuaXuLy.Fail("So ngay khong duoc am");
            if (soNgayCong + nghiPhep + nghiKP > soNgayTrongThang)
                return KetQuaXuLy.Fail("Tong ngay cong va ngay nghi vuot qua so ngay trong thang (" + soNgayTrongThang + ")");

            try
            {
                int n = Db.Execute(@"UPDATE BangChamCong SET SoNgayCong=@c, SoNgayNghiPhep=@p, SoNgayNghiKP=@k
                                     WHERE MaNV=@m AND Thang=@t AND Nam=@n",
                    Db.P("@c", soNgayCong), Db.P("@p", nghiPhep), Db.P("@k", nghiKP),
                    Db.P("@m", maNV), Db.P("@t", thang), Db.P("@n", nam));
                if (n > 0) return KetQuaXuLy.Ok("Cap nhat bang cham cong thanh cong");

                Db.Execute(@"INSERT INTO BangChamCong(MaBCC, MaNV, Thang, Nam, SoNgayCong, SoNgayNghiPhep, SoNgayNghiKP)
                             VALUES(@ma,@m,@t,@n,@c,@p,@k)",
                    Db.P("@ma", "CC" + (nam % 100).ToString("00") + thang.ToString("00") + maNV),
                    Db.P("@m", maNV), Db.P("@t", thang), Db.P("@n", nam),
                    Db.P("@c", soNgayCong), Db.P("@p", nghiPhep), Db.P("@k", nghiKP));
                return KetQuaXuLy.Ok("Luu bang cham cong thanh cong");
            }
            catch (Exception ex) { return KetQuaXuLy.Fail("Loi khi luu cham cong: " + ex.Message); }
        }

        public static DataTable DsChamCong(int thang, int nam)
            => Db.Query(@"SELECT cc.MaNV, nv.HoTen, cc.SoNgayCong, cc.SoNgayNghiPhep, cc.SoNgayNghiKP
                          FROM BangChamCong cc JOIN NhanVien nv ON cc.MaNV = nv.MaNV
                          WHERE cc.Thang=@t AND cc.Nam=@n ORDER BY cc.MaNV", Db.P("@t", thang), Db.P("@n", nam));

        // Phieu ghi nhan gio phu troi (tang ca, ngay le, ngay nghi) kem chi tiet tung nhan vien.
        public static KetQuaXuLy LapPhieuPhuTroi(DateTime ngay, string hinhThuc, string maPB, string nguoiLap, string ghiChu,
            List<ChiTietPhuTroiItem> ds)
        {
            if (hinhThuc != QuyDinh.TangCa && hinhThuc != QuyDinh.NgayLe && hinhThuc != QuyDinh.NgayNghi)
                return KetQuaXuLy.Fail("Hinh thuc phu troi khong hop le");
            if (string.IsNullOrWhiteSpace(maPB)) return KetQuaXuLy.Fail("Chon phong ban");
            if (string.IsNullOrWhiteSpace(nguoiLap)) return KetQuaXuLy.Fail("Nhap nguoi lap phieu");
            if (ds == null || ds.Count == 0) return KetQuaXuLy.Fail("Phieu phu troi phai co it nhat 1 nhan vien");
            if (ds.Any(x => x.SoGio <= 0 || x.SoGio > 12)) return KetQuaXuLy.Fail("So gio phu troi phai trong khoang (0, 12]");
            if (ds.GroupBy(x => x.MaNV).Any(g => g.Count() > 1)) return KetQuaXuLy.Fail("Moi nhan vien chi ghi 1 dong tren phieu");

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        string soPhieu = QuyDinh.SinhMa("PT");
                        Db.ExecuteInTransaction(cn, tran,
                            "INSERT INTO PhieuPhuTroi(SoPhieu, NgayPhuTroi, HinhThuc, MaPB, NguoiLap, GhiChu) VALUES(@s,@n,@h,@pb,@nl,@gc)",
                            Db.P("@s", soPhieu), Db.P("@n", ngay.Date), Db.P("@h", hinhThuc), Db.P("@pb", maPB),
                            Db.P("@nl", nguoiLap), Db.P("@gc", ghiChu));
                        foreach (var ct in ds)
                        {
                            Db.ExecuteInTransaction(cn, tran,
                                "INSERT INTO ChiTietPhuTroi(SoPhieu, MaNV, SoGio) VALUES(@s,@nv,@g)",
                                Db.P("@s", soPhieu), Db.P("@nv", ct.MaNV), Db.P("@g", ct.SoGio));
                        }
                        tran.Commit();
                        return KetQuaXuLy.Ok("Lap phieu phu troi thanh cong. So phieu: " + soPhieu, soPhieu);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi lap phieu phu troi: " + ex.Message);
                    }
                }
            }
        }

        public static DataTable DsPhuTroi(int thang, int nam)
            => Db.Query(@"SELECT p.SoPhieu, p.NgayPhuTroi, p.HinhThuc, ct.MaNV, nv.HoTen, ct.SoGio
                          FROM PhieuPhuTroi p
                          JOIN ChiTietPhuTroi ct ON p.SoPhieu = ct.SoPhieu
                          JOIN NhanVien nv ON ct.MaNV = nv.MaNV
                          WHERE MONTH(p.NgayPhuTroi)=@t AND YEAR(p.NgayPhuTroi)=@n
                          ORDER BY p.NgayPhuTroi, p.SoPhieu", Db.P("@t", thang), Db.P("@n", nam));
    }
}
