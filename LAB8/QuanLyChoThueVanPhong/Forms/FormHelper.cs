using System;
using System.Data;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;

namespace QuanLyChoThueVanPhong.Forms
{
    public static class FormHelper
    {
        public static void Nap(ComboBox cbo, DataTable dt, string display, string value)
        {
            // dat DisplayMember/ValueMember truoc DataSource de su kien SelectedIndexChanged doc dung SelectedValue
            cbo.DisplayMember = display;
            cbo.ValueMember = value;
            cbo.DataSource = dt;
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

        // Doc so tien tu o nhap; o trong duoc xem la 0.
        public static bool Tien(TextBox txt, string tenTruong, out decimal giaTri)
        {
            giaTri = 0;
            string s = txt.Text.Replace(",", "").Replace(".", "").Trim();
            if (s.Length == 0) return true;
            if (decimal.TryParse(s, out giaTri) && giaTri >= 0) return true;
            MessageBox.Show(tenTruong + " khong hop le");
            txt.Focus();
            return false;
        }

        public static void DinhDangTien(DataGridView grid)
        {
            foreach (DataGridViewColumn c in grid.Columns)
                if (c.ValueType == typeof(decimal)) c.DefaultCellStyle.Format = "N0";
        }

        // ---- Tao control nhanh cho cac file Designer ----
        public static Label L(string text, int x, int y, int w = 100) { var c = new Label { Text = text }; c.SetBounds(x, y + 3, w, 23); return c; }
        public static TextBox T(int x, int y, int w) { var c = new TextBox(); c.SetBounds(x, y, w, 23); return c; }
        public static ComboBox C(int x, int y, int w) { var c = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; c.SetBounds(x, y, w, 23); return c; }
        public static DateTimePicker D(int x, int y, int w = 120) { var c = new DateTimePicker { Format = DateTimePickerFormat.Short }; c.SetBounds(x, y, w, 23); return c; }
        public static NumericUpDown N(int x, int y, int w, decimal min, decimal max, int le = 0)
        { var c = new NumericUpDown { Minimum = min, Maximum = max, DecimalPlaces = le }; c.SetBounds(x, y, w, 23); return c; }
        public static Button B(string text, int x, int y, int w, EventHandler click, int h = 28)
        { var c = new Button { Text = text }; c.SetBounds(x, y, w, h); c.Click += click; return c; }
        public static DataGridView G(int x, int y, int w, int h)
        {
            var c = new DataGridView { ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false };
            c.SetBounds(x, y, w, h);
            c.DataBindingComplete += (s, e) => DinhDangTien(c);
            return c;
        }
    }
}
