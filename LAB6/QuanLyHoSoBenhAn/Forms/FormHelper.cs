using System;
using System.Data;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Services;

namespace QuanLyHoSoBenhAn.Forms
{
    public static class FormHelper
    {
        public static void Nap(ComboBox cbo, DataTable dt, string display, string value)
        {
            cbo.DataSource = dt;
            cbo.DisplayMember = display;
            cbo.ValueMember = value;
            if (dt.Rows.Count > 0) cbo.SelectedIndex = 0;
        }

        public static string Gia(ComboBox cbo)
        {
            if (cbo.SelectedValue == null) return null;
            return cbo.SelectedValue.ToString();
        }

        public static string O(DataGridView grid, string cot)
        {
            if (grid.CurrentRow == null) return null;
            var cell = grid.CurrentRow.Cells[cot];
            return cell?.Value?.ToString();
        }

        public static bool Bao(KetQuaXuLy kq)
        {
            if (kq.ThanhCong)
            {
                MessageBox.Show(kq.ThongBao, "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            MessageBox.Show(kq.ThongBao, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
    }
}
