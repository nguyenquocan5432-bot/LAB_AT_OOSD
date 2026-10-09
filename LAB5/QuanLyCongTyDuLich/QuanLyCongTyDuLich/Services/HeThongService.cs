using System;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public sealed class HeThongService
    {
        public int KiemTraKetNoi()
        {
            return Convert.ToInt32(Db.Scalar("SELECT COUNT(*) FROM dbo.Tour;"));
        }
    }
}
