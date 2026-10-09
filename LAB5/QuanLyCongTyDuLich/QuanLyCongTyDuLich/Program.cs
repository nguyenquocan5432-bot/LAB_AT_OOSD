using System;
using System.Windows.Forms;
using QuanLyCongTyDuLich.UI;

namespace QuanLyCongTyDuLich
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
