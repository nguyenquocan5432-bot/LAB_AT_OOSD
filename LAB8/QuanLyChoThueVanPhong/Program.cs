using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Forms;

namespace QuanLyChoThueVanPhong
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
