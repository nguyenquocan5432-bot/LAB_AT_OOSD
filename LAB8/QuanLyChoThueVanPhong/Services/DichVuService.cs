using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using QuanLyChoThueVanPhong.Data;

namespace QuanLyChoThueVanPhong.Services
{
    public static class DichVuService
    {
        // Lap hoa don dich vu cuoi thang (dien, nuoc, internet...) cho mot hop dong: hoa don + chi tiet trong 1 transaction.
        public static KetQuaXuLy LapHoaDon(string soHD, int thang, int nam, List<ChiTietDichVuItem> ds)
        {
            if (string.IsNullOrWhiteSpace(soHD)) return KetQuaXuLy.Fail("Chon hop dong");
            if (thang < 1 || thang > 12 || nam < 2000) return KetQuaXuLy.Fail("Thang/nam khong hop le");
            if (ds == null || ds.Count == 0) return KetQuaXuLy.Fail("Hoa don phai co it nhat 1 dich vu");
            if (ds.Any(x => x.SoLuong <= 0)) return KetQuaXuLy.Fail("So luong su dung phai lon hon 0");
            if (ds.GroupBy(x => x.MaDV).Any(g => g.Count() > 1)) return KetQuaXuLy.Fail("Moi dich vu chi ghi 1 dong");
            DateTime dauThang = new DateTime(nam, thang, 1);
            if (dauThang > DateTime.Today) return KetQuaXuLy.Fail("Khong lap hoa don cho thang chua dien ra");

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        DataTable hd = Db.QueryInTransaction(cn, tran,
                            "SELECT NgayBatDau, NgayKetThuc, TrangThai FROM HopDongThue WHERE SoHD=@hd", Db.P("@hd", soHD));
                        if (hd.Rows.Count == 0 || hd.Rows[0]["TrangThai"].ToString() != QuyDinh.HDHieuLuc)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Hop dong khong ton tai hoac da thanh ly"); }
                        DateTime bd = (DateTime)hd.Rows[0]["NgayBatDau"], kt = (DateTime)hd.Rows[0]["NgayKetThuc"];
                        if (dauThang.AddMonths(1) <= bd || dauThang >= kt)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Thang " + thang + "/" + nam + " nam ngoai thoi gian thue"); }
                        object trung = Db.ScalarInTransaction(cn, tran,
                            "SELECT COUNT(*) FROM HoaDonDichVu WHERE SoHD=@hd AND Thang=@t AND Nam=@n",
                            Db.P("@hd", soHD), Db.P("@t", thang), Db.P("@n", nam));
                        if (Convert.ToInt32(trung) > 0)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Hop dong da co hoa don dich vu thang " + thang + "/" + nam); }

                        decimal tong = ds.Sum(x => x.ThanhTien);
                        string ma = QuyDinh.SinhMa("HDV");
                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO HoaDonDichVu(MaHoaDon, SoHD, Thang, Nam, NgayLap, TongTien, TrangThai)
                              VALUES(@m,@hd,@t,@n,@nl,@tong,@tt)",
                            Db.P("@m", ma), Db.P("@hd", soHD), Db.P("@t", thang), Db.P("@n", nam), Db.P("@nl", DateTime.Today),
                            Db.P("@tong", tong), Db.P("@tt", QuyDinh.ChuaThanhToan));
                        foreach (var ct in ds)
                        {
                            Db.ExecuteInTransaction(cn, tran,
                                "INSERT INTO ChiTietHoaDonDV(MaHoaDon, MaDV, SoLuong, DonGia, ThanhTien) VALUES(@m,@dv,@sl,@dg,@tt)",
                                Db.P("@m", ma), Db.P("@dv", ct.MaDV), Db.P("@sl", ct.SoLuong), Db.P("@dg", ct.DonGia), Db.P("@tt", ct.ThanhTien));
                        }
                        tran.Commit();
                        return KetQuaXuLy.Ok("Lap hoa don dich vu thanh cong. Ma: " + ma + ", tong tien: " + tong.ToString("N0") + " d", ma);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi lap hoa don: " + ex.Message);
                    }
                }
            }
        }

        public static KetQuaXuLy ThanhToanHoaDon(string maHoaDon)
        {
            if (string.IsNullOrWhiteSpace(maHoaDon)) return KetQuaXuLy.Fail("Chon hoa don");
            int n = Db.Execute("UPDATE HoaDonDichVu SET TrangThai=@da, NgayThanhToan=@n WHERE MaHoaDon=@m AND TrangThai=@chua",
                Db.P("@da", QuyDinh.DaThanhToan), Db.P("@n", DateTime.Today), Db.P("@m", maHoaDon), Db.P("@chua", QuyDinh.ChuaThanhToan));
            return n > 0 ? KetQuaXuLy.Ok("Thanh toan hoa don " + maHoaDon + " thanh cong")
                         : KetQuaXuLy.Fail("Hoa don khong ton tai hoac da thanh toan");
        }

        public static DataTable DsHoaDon(string soHD)
            => Db.Query(@"SELECT MaHoaDon, Thang, Nam, NgayLap, TongTien, TrangThai, NgayThanhToan
                          FROM HoaDonDichVu WHERE SoHD=@hd ORDER BY Nam DESC, Thang DESC", Db.P("@hd", soHD));

        public static DataTable ChiTietHoaDon(string maHoaDon)
            => Db.Query(@"SELECT dv.TenDV, dv.DonViTinh, ct.SoLuong, ct.DonGia, ct.ThanhTien
                          FROM ChiTietHoaDonDV ct JOIN DichVu dv ON ct.MaDV = dv.MaDV
                          WHERE ct.MaHoaDon=@m", Db.P("@m", maHoaDon));
    }
}
