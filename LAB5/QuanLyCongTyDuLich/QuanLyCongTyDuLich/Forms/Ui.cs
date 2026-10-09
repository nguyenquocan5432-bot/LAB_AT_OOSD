using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace QuanLyCongTyDuLich.UI
{
    internal static class Ui
    {
        public static void Setup(Form f, string title, int width = 1160, int height = 640)
        {
            f.Text = title; f.Font = new Font("Segoe UI", 9F);
            f.BackColor = SystemColors.Control;
            f.AutoScaleDimensions = new SizeF(96F, 96F); f.AutoScaleMode = AutoScaleMode.Dpi;
            f.ClientSize = new Size(width, height); f.MinimumSize = new Size(width, height);
            f.StartPosition = FormStartPosition.CenterParent;
        }
        public static Panel Body(Form f)
        {
            var footer = new Panel { Dock = DockStyle.Bottom, Height = 46 };
            var close = Button("Đóng", () => f.Close(), 90); close.Location = new Point(f.ClientSize.Width - 106, 8);
            close.Anchor = AnchorStyles.Top | AnchorStyles.Right; footer.Controls.Add(close);
            var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            f.Controls.Add(body); f.Controls.Add(footer); return body;
        }
        public static Label Label(string text, bool bold = false)
        {
            return new Label { Text = text, AutoSize = false, TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, bold ? FontStyle.Bold : FontStyle.Regular) };
        }
        public static TextBox Text(string value = "", int max = 200) { return new TextBox { Text = value, MaxLength = max }; }
        public static ComboBox Combo() { return new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; }
        public static NumericUpDown Number(decimal value = 0, decimal max = 100000, int digits = 0)
        {
            return new NumericUpDown { Minimum = 0, Maximum = max, Value = value, DecimalPlaces = digits, ThousandsSeparator = true };
        }
        public static NumericUpDown Money(decimal value = 0) { return Number(value, 9999999999999999.99m, 2); }
        public static DateTimePicker Date(DateTime? value = null)
        {
            return new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Value = value ?? DateTime.Today };
        }
        public static Button Button(string text, Action action, int width = 140)
        {
            var b = new Button { Text = text, Size = new Size(width, 30), UseVisualStyleBackColor = true };
            if (action != null) b.Click += (s, e) => Safe(b.FindForm(), action); return b;
        }
        public static void Place(Control parent, Control input, int x, int y, int width, int height = 26)
        {
            input.SetBounds(x, y, width, height); parent.Controls.Add(input);
        }
        public static void Field(Control p, string text, Control c, int x, int y, int width = 240, int labelWidth = 100)
        {
            Place(p, Label(text), x, y, labelWidth, 26); Place(p, c, x + labelWidth, y + 2, width);
        }
        public static DataGridView Grid(bool editable = false)
        {
            var g = new DataGridView { Dock = DockStyle.Fill, BackgroundColor = SystemColors.AppWorkspace,
                ReadOnly = !editable, AllowUserToAddRows = editable, AllowUserToDeleteRows = editable,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                SelectionMode = editable ? DataGridViewSelectionMode.CellSelect : DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false, RowHeadersWidth = 35, EnableHeadersVisualStyles = false,
                BorderStyle = BorderStyle.FixedSingle };
            g.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Window;
            g.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            g.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            g.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
            g.DataBindingComplete += (s, e) => {
                foreach (DataGridViewColumn col in g.Columns)
                {
                    if (col.ValueType == typeof(DateTime)) col.DefaultCellStyle.Format = "dd/MM/yyyy";
                    if (col.ValueType == typeof(decimal)) { col.DefaultCellStyle.Format = "N0"; col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }
                    if (col.Name == "HienThi") col.Visible = false;
                }
            };
            return g;
        }
        public static void Bind(ComboBox c, DataTable dt, string id, string display)
        { c.DataSource = null; c.DisplayMember = display; c.ValueMember = id; c.DataSource = dt; }
        public static string Key(ComboBox c) { return c.SelectedValue is string ? (string)c.SelectedValue : ""; }
        public static DataRow Row(ComboBox c) { var v = c.SelectedItem as DataRowView; return v == null ? null : v.Row; }
        public static string Cell(DataGridView g, string key)
        { return g.CurrentRow == null ? "" : Convert.ToString(g.CurrentRow.Cells[key].Value); }
        public static string Id(string prefix) { return prefix + Guid.NewGuid().ToString("N").Substring(0, 20 - prefix.Length); }
        public static void Safe(Form owner, Action action)
        {
            try { action(); }
            catch (Exception ex) {
                string message = ex is ArgumentException || ex is InvalidOperationException ? ex.Message
                    : "Không hoàn tất thao tác. Kiểm tra SQL Server, chuỗi kết nối App.config và các script khởi tạo/nâng cấp CSDL.";
                MessageBox.Show(owner, message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public static void Done(Form owner, string text = "Đã lưu dữ liệu.")
        { MessageBox.Show(owner, text, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        public static string Amount(decimal n) { return n.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")); }
        public static Panel Editor(TabPage tab, int height)
        {
            var editor = new Panel { Dock = DockStyle.Bottom, Height = height };
            tab.Padding = new Padding(8); tab.Controls.Add(editor); return editor;
        }
    }
}
