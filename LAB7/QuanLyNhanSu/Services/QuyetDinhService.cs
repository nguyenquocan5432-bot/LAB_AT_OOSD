using System;
using System.Data;
using QuanLyNhanSu.Data;

namespace QuanLyNhanSu.Services
{
    public static class QuyetDinhService
    {
        // Quyet dinh tang luong cho nhan vien chinh thuc.
        public static KetQuaXuLy LapQuyetDinhLuong(string maNV, DateTime ngayHieuLuc, decimal luongCoBan,
            decimal phuCapLuong, decimal phuCapDocHai, string nguoiKy, string ghiChu)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return KetQuaXuLy.Fail("Chon nhan vien");
            if (luongCoBan <= 0) return KetQuaXuLy.Fail("Luong co ban phai lon hon 0");
            if (phuCapLuong < 0 || phuCapDocHai < 0) return KetQuaXuLy.Fail("Phu cap khong duoc am");
            if (string.IsNullOrWhiteSpace(nguoiKy)) return KetQuaXuLy.Fail("Nhap nguoi ky");
            if (!LaChinhThuc(maNV)) return KetQuaXuLy.Fail("Chi lap quyet dinh luong cho nhan vien chinh thuc");

            object hlCu = Db.Scalar("SELECT MAX(NgayHieuLuc) FROM QuyetDinhLuong WHERE MaNV=@m", Db.P("@m", maNV));
            if (hlCu != null && hlCu != DBNull.Value && ngayHieuLuc.Date <= (DateTime)hlCu)
                return KetQuaXuLy.Fail("Ngay hieu luc phai sau quyet dinh luong gan nhat (" + ((DateTime)hlCu).ToString("dd/MM/yyyy") + ")");

            string soQD = QuyDinh.SinhMa("QDL");
            try
            {
                Db.Execute(@"INSERT INTO QuyetDinhLuong(SoQD, MaNV, NgayKy, NgayHieuLuc, LuongCoBan, PhuCapLuong, PhuCapDocHai, NguoiKy, GhiChu)
                             VALUES(@qd,@m,@nk,@hl,@lcb,@pcl,@pcdh,@ky,@gc)",
                    Db.P("@qd", soQD), Db.P("@m", maNV), Db.P("@nk", DateTime.Today), Db.P("@hl", ngayHieuLuc.Date),
                    Db.P("@lcb", luongCoBan), Db.P("@pcl", phuCapLuong), Db.P("@pcdh", phuCapDocHai),
                    Db.P("@ky", nguoiKy), Db.P("@gc", ghiChu));
            }
            catch (Exception ex) { return KetQuaXuLy.Fail("Loi khi lap quyet dinh luong: " + ex.Message); }
            return KetQuaXuLy.Ok("Lap quyet dinh luong thanh cong. So QD: " + soQD, soQD);
        }

        // Quyet dinh bo nhiem chuc vu va phu cap chuc vu.
        public static KetQuaXuLy LapQuyetDinhChucVu(string maNV, string maCV, DateTime ngayHieuLuc, decimal phuCap, string nguoiKy)
        {
            if (string.IsNullOrWhiteSpace(maNV) || string.IsNullOrWhiteSpace(maCV)) return KetQuaXuLy.Fail("Chon nhan vien va chuc vu");
            if (phuCap < 0) return KetQuaXuLy.Fail("Phu cap chuc vu khong duoc am");
            if (string.IsNullOrWhiteSpace(nguoiKy)) return KetQuaXuLy.Fail("Nhap nguoi ky");
            if (!LaChinhThuc(maNV)) return KetQuaXuLy.Fail("Chi bo nhiem chuc vu cho nhan vien chinh thuc");

            string soQD = QuyDinh.SinhMa("QDCV");
            try
            {
                Db.Execute(@"INSERT INTO QuyetDinhChucVu(SoQD, MaNV, MaCV, NgayKy, NgayHieuLuc, PhuCapChucVu, NguoiKy)
                             VALUES(@qd,@m,@cv,@nk,@hl,@pc,@ky)",
                    Db.P("@qd", soQD), Db.P("@m", maNV), Db.P("@cv", maCV), Db.P("@nk", DateTime.Today),
                    Db.P("@hl", ngayHieuLuc.Date), Db.P("@pc", phuCap), Db.P("@ky", nguoiKy));
            }
            catch (Exception ex) { return KetQuaXuLy.Fail("Loi khi lap quyet dinh chuc vu: " + ex.Message); }
            return KetQuaXuLy.Ok("Lap quyet dinh chuc vu thanh cong. So QD: " + soQD, soQD);
        }

        // Qua trinh tang luong, bo nhiem cua mot nhan vien (ghi nhan qua cac quyet dinh).
        public static DataTable LichSuQuyetDinh(string maNV)
            => Db.Query(@"SELECT 'Tuyen dung' AS LoaiQD, SoQD, NgayKy, NgayBatDauTV AS NgayHieuLuc,
                                 N'Thu viec den ' + CONVERT(VARCHAR(10), NgayKetThucTV, 103) + N', luong ' + FORMAT(LuongThuViec, 'N0') AS NoiDung
                          FROM QuyetDinhTuyenDung WHERE MaNV=@m
                          UNION ALL
                          SELECT 'Luong', SoQD, NgayKy, NgayHieuLuc,
                                 N'LCB ' + FORMAT(LuongCoBan, 'N0') + N', PC luong ' + FORMAT(PhuCapLuong, 'N0') + N', PC doc hai ' + FORMAT(PhuCapDocHai, 'N0')
                          FROM QuyetDinhLuong WHERE MaNV=@m
                          UNION ALL
                          SELECT 'Chuc vu', q.SoQD, q.NgayKy, q.NgayHieuLuc, cv.TenCV + N', PC chuc vu ' + FORMAT(q.PhuCapChucVu, 'N0')
                          FROM QuyetDinhChucVu q JOIN ChucVu cv ON q.MaCV = cv.MaCV WHERE q.MaNV=@m
                          ORDER BY NgayHieuLuc", Db.P("@m", maNV));

        private static bool LaChinhThuc(string maNV)
        {
            object tt = Db.Scalar("SELECT TrangThai FROM NhanVien WHERE MaNV=@m", Db.P("@m", maNV));
            return tt != null && tt.ToString() == QuyDinh.ChinhThuc;
        }
    }
}
