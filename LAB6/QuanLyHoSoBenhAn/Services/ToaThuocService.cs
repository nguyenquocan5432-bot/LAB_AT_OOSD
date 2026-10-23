using System;
using System.Collections.Generic;
using System.Data;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Services
{
    public static class ToaThuocService
    {
        // Lap toa thuoc kem nhieu chi tiet, trong 1 transaction.
        // Moi phieu kham chi duoc lap 1 toa thuoc (rang buoc UNIQUE MaPhieuKham tren bang ToaThuoc).
        public static KetQuaXuLy LapToa(string maPhieuKham, string maBacSi, string ghiChu, List<ChiTietToaItem> dsChiTiet)
        {
            if (string.IsNullOrWhiteSpace(maPhieuKham)) return KetQuaXuLy.Fail("Chua chon phieu kham");
            if (dsChiTiet == null || dsChiTiet.Count == 0) return KetQuaXuLy.Fail("Toa thuoc phai co it nhat 1 loai thuoc");

            foreach (var ct in dsChiTiet)
            {
                if (string.IsNullOrWhiteSpace(ct.MaThuoc) || ct.SoLuong <= 0)
                    return KetQuaXuLy.Fail("Thong tin thuoc khong hop le: " + ct.TenThuoc);
            }

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        object daCoToa = Db.ScalarInTransaction(cn, tran,
                            "SELECT COUNT(*) FROM ToaThuoc WHERE MaPhieuKham=@m", Db.P("@m", maPhieuKham));
                        if (Convert.ToInt32(daCoToa) > 0)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Phieu kham nay da duoc lap toa thuoc"); }

                        string maToa = "TOA" + DateTime.Now.ToString("yyMMddHHmmss");
                        Db.ExecuteInTransaction(cn, tran,
                            "INSERT INTO ToaThuoc(MaToa, MaPhieuKham, NgayLap, MaBacSi, GhiChu) VALUES(@t,@p,@n,@b,@g)",
                            Db.P("@t", maToa), Db.P("@p", maPhieuKham), Db.P("@n", DateTime.Now),
                            Db.P("@b", maBacSi), Db.P("@g", ghiChu));

                        foreach (var ct in dsChiTiet)
                        {
                            Db.ExecuteInTransaction(cn, tran,
                                "INSERT INTO ChiTietToaThuoc(MaToa, MaThuoc, LieuDung, SoLuong) VALUES(@t,@th,@l,@s)",
                                Db.P("@t", maToa), Db.P("@th", ct.MaThuoc), Db.P("@l", ct.LieuDung), Db.P("@s", ct.SoLuong));
                        }

                        Db.ExecuteInTransaction(cn, tran,
                            "UPDATE PhieuKhamBenh SET TrangThai=@tt WHERE MaPhieuKham=@m",
                            Db.P("@tt", QuyDinh.DaLapToa), Db.P("@m", maPhieuKham));

                        tran.Commit();
                        return KetQuaXuLy.Ok("Lap toa thuoc thanh cong. Ma toa: " + maToa, maToa);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi lap toa: " + ex.Message);
                    }
                }
            }
        }

        public static DataTable ChiTietToa(string maPhieuKham)
            => Db.Query(@"SELECT t.TenThuoc, ct.LieuDung, ct.SoLuong
                          FROM ChiTietToaThuoc ct
                          JOIN ToaThuoc tt ON ct.MaToa = tt.MaToa
                          JOIN Thuoc t ON ct.MaThuoc = t.MaThuoc
                          WHERE tt.MaPhieuKham = @m", Db.P("@m", maPhieuKham));
    }
}
