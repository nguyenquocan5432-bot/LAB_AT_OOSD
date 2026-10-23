using System;
using System.Data;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Services
{
    public static class ThanhToanService
    {
        // Kham dinh ky duoc mien phi, chi khoi tao lenh thanh toan cho kham dieu tri
        public static KetQuaXuLy XuatVienThanhToan(string maPhieuKham, decimal soTien)
        {
            var dt = Db.Query("SELECT LoaiKham, TrangThai FROM PhieuKhamBenh WHERE MaPhieuKham=@m", Db.P("@m", maPhieuKham));
            if (dt.Rows.Count == 0) return KetQuaXuLy.Fail("Khong tim thay phieu kham");

            string loai = dt.Rows[0]["LoaiKham"].ToString();
            if (loai == QuyDinh.DinhKy)
                return KetQuaXuLy.Fail("Kham dinh ky duoc mien phi, khong can thanh toan");

            if (soTien < 0) return KetQuaXuLy.Fail("So tien khong hop le");

            if (Db.Scalar("SELECT COUNT(*) FROM ThanhToanVienPhi WHERE MaPhieuKham=@m", Db.P("@m", maPhieuKham)) is int c && c > 0)
                return KetQuaXuLy.Fail("Phieu kham nay da duoc thanh toan");

            string maTT = "TT" + DateTime.Now.ToString("yyMMddHHmmss");
            Db.Execute("INSERT INTO ThanhToanVienPhi(MaThanhToan, MaPhieuKham, SoTien, NgayThanhToan) VALUES(@t,@p,@s,@n)",
                Db.P("@t", maTT), Db.P("@p", maPhieuKham), Db.P("@s", soTien), Db.P("@n", DateTime.Now));

            Db.Execute("UPDATE PhieuKhamBenh SET TrangThai=@tt WHERE MaPhieuKham=@m",
                Db.P("@tt", QuyDinh.DaXuatVien), Db.P("@m", maPhieuKham));

            return KetQuaXuLy.Ok("Thanh toan vien phi va xuat vien thanh cong");
        }

        public static KetQuaXuLy XacNhanXuatVienMienPhi(string maPhieuKham)
        {
            var dt = Db.Query("SELECT LoaiKham FROM PhieuKhamBenh WHERE MaPhieuKham=@m", Db.P("@m", maPhieuKham));
            if (dt.Rows.Count == 0) return KetQuaXuLy.Fail("Khong tim thay phieu kham");
            if (dt.Rows[0]["LoaiKham"].ToString() != QuyDinh.DinhKy)
                return KetQuaXuLy.Fail("Chi ap dung cho phieu kham dinh ky");

            Db.Execute("UPDATE PhieuKhamBenh SET TrangThai=@tt WHERE MaPhieuKham=@m",
                Db.P("@tt", QuyDinh.DaXuatVien), Db.P("@m", maPhieuKham));
            return KetQuaXuLy.Ok("Xac nhan hoan tat kham dinh ky (mien phi)");
        }
    }
}
