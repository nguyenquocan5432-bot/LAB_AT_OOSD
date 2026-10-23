using System;
using System.Data;
using QuanLyNhanSu.Data;

namespace QuanLyNhanSu.Services
{
    public static class TuyenDungService
    {
        // Lap quyet dinh tuyen dung: them nhan vien (trang thai Thu viec) + quyet dinh tuyen dung
        // + giam nhu cau tuyen cua phong ban, tat ca trong 1 transaction.
        public static KetQuaXuLy LapQuyetDinhTuyenDung(NhanVienMoi nv, ThongTinTuyenDung qd)
        {
            if (nv == null || qd == null) return KetQuaXuLy.Fail("Thieu thong tin tuyen dung");
            if (string.IsNullOrWhiteSpace(nv.HoTen)) return KetQuaXuLy.Fail("Nhap ho ten nhan vien");
            if (string.IsNullOrWhiteSpace(nv.CCCD) || nv.CCCD.Trim().Length != 12) return KetQuaXuLy.Fail("So CCCD phai gom 12 chu so");
            if (nv.NgaySinh > DateTime.Today.AddYears(-18)) return KetQuaXuLy.Fail("Nhan vien phai du 18 tuoi");
            if (string.IsNullOrWhiteSpace(qd.MaPB)) return KetQuaXuLy.Fail("Chon phong ban tuyen dung");
            if (qd.NgayKetThucTV <= qd.NgayBatDauTV) return KetQuaXuLy.Fail("Ngay ket thuc thu viec phai sau ngay bat dau");
            if ((qd.NgayKetThucTV - qd.NgayBatDauTV).TotalDays > QuyDinh.SoNgayThuViecToiDa)
                return KetQuaXuLy.Fail("Thoi gian thu viec toi da " + QuyDinh.SoNgayThuViecToiDa + " ngay");
            if (qd.LuongThuViec <= 0) return KetQuaXuLy.Fail("Luong thu viec phai lon hon 0");
            if (string.IsNullOrWhiteSpace(qd.DieuKhoanCongTy) || string.IsNullOrWhiteSpace(qd.DieuKhoanNhanVien))
                return KetQuaXuLy.Fail("Quyet dinh phai ghi ro dieu khoan cua hai ben");
            if (string.IsNullOrWhiteSpace(qd.NguoiKy)) return KetQuaXuLy.Fail("Nhap nguoi ky quyet dinh");

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        object nhuCau = Db.ScalarInTransaction(cn, tran,
                            "SELECT NhuCauTuyen FROM PhongBan WHERE MaPB=@pb", Db.P("@pb", qd.MaPB));
                        if (nhuCau == null || Convert.ToInt32(nhuCau) <= 0)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Phong ban nay hien khong co nhu cau tuyen dung"); }

                        object trungCccd = Db.ScalarInTransaction(cn, tran,
                            "SELECT COUNT(*) FROM NhanVien WHERE CCCD=@c", Db.P("@c", nv.CCCD.Trim()));
                        if (Convert.ToInt32(trungCccd) > 0)
                        { tran.Rollback(); return KetQuaXuLy.Fail("So CCCD nay da co trong ho so nhan vien"); }

                        string maNV = QuyDinh.SinhMa("NV");
                        string soQD = QuyDinh.SinhMa("QDTD");

                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO NhanVien(MaNV, HoTen, NgaySinh, GioiTinh, CCCD, DiaChi, DienThoai, MaPB, NgayVaoLam, TrangThai)
                              VALUES(@m,@ht,@ns,@gt,@c,@dc,@dt,@pb,@nvl,@tt)",
                            Db.P("@m", maNV), Db.P("@ht", nv.HoTen.Trim()), Db.P("@ns", nv.NgaySinh.Date),
                            Db.P("@gt", nv.GioiTinh), Db.P("@c", nv.CCCD.Trim()), Db.P("@dc", nv.DiaChi),
                            Db.P("@dt", nv.DienThoai), Db.P("@pb", qd.MaPB), Db.P("@nvl", qd.NgayBatDauTV.Date),
                            Db.P("@tt", QuyDinh.ThuViec));

                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO QuyetDinhTuyenDung(SoQD, MaNV, MaPB, NgayKy, NgayBatDauTV, NgayKetThucTV, LuongThuViec,
                                                             DieuKhoanCongTy, DieuKhoanNhanVien, NguoiKy)
                              VALUES(@qd,@m,@pb,@nk,@bd,@kt,@l,@dk1,@dk2,@ky)",
                            Db.P("@qd", soQD), Db.P("@m", maNV), Db.P("@pb", qd.MaPB), Db.P("@nk", DateTime.Today),
                            Db.P("@bd", qd.NgayBatDauTV.Date), Db.P("@kt", qd.NgayKetThucTV.Date), Db.P("@l", qd.LuongThuViec),
                            Db.P("@dk1", qd.DieuKhoanCongTy), Db.P("@dk2", qd.DieuKhoanNhanVien), Db.P("@ky", qd.NguoiKy));

                        Db.ExecuteInTransaction(cn, tran,
                            "UPDATE PhongBan SET NhuCauTuyen = NhuCauTuyen - 1 WHERE MaPB=@pb", Db.P("@pb", qd.MaPB));

                        tran.Commit();
                        return KetQuaXuLy.Ok("Lap quyet dinh tuyen dung thanh cong. So QD: " + soQD + ", ma NV: " + maNV, maNV);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi lap quyet dinh tuyen dung: " + ex.Message);
                    }
                }
            }
        }

        // Danh gia thu viec khong dat: cham dut thu viec, nhan vien chuyen "Nghi viec".
        public static KetQuaXuLy ChamDutThuViec(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return KetQuaXuLy.Fail("Chon nhan vien thu viec");
            int n = Db.Execute("UPDATE NhanVien SET TrangThai=@nghi WHERE MaNV=@m AND TrangThai=@tv",
                Db.P("@nghi", QuyDinh.NghiViec), Db.P("@m", maNV), Db.P("@tv", QuyDinh.ThuViec));
            return n > 0 ? KetQuaXuLy.Ok("Da cham dut thu viec cua nhan vien " + maNV)
                         : KetQuaXuLy.Fail("Nhan vien khong o trang thai thu viec");
        }

        public static DataTable DsThuViec()
            => Db.Query(@"SELECT nv.MaNV, nv.HoTen, pb.TenPB, qd.SoQD, qd.NgayBatDauTV, qd.NgayKetThucTV, qd.LuongThuViec,
                                 CASE WHEN qd.NgayKetThucTV < CAST(GETDATE() AS DATE) THEN 'Het thoi gian thu viec'
                                      ELSE 'Dang thu viec' END AS TinhTrang
                          FROM NhanVien nv
                          JOIN QuyetDinhTuyenDung qd ON nv.MaNV = qd.MaNV
                          JOIN PhongBan pb ON qd.MaPB = pb.MaPB
                          WHERE nv.TrangThai = @tv
                          ORDER BY qd.NgayKetThucTV", Db.P("@tv", QuyDinh.ThuViec));
    }
}
