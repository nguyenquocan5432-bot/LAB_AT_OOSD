using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EShopping
{
    // ===== Tầng UI (WinForms, dựng bằng code, không dùng Designer) =====

    static class Ui
    {
        public static void Info(string m) { MessageBox.Show(m, "e-SHOPPING", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        public static void Error(string m) { MessageBox.Show(m, "e-SHOPPING", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

        // Chạy một hành động, hiển thị lỗi nghiệp vụ / lỗi hệ thống thân thiện
        public static bool Try(Action a)
        {
            try { a(); return true; }
            catch (BusinessException ex) { Error(ex.Message); }
            catch (Exception ex)
            {
                Error("Lỗi hệ thống (kiểm tra kết nối CSDL SQL Server):\n" + ex.Message);
            }
            return false;
        }

        public static TableLayoutPanel Form2Col()
        {
            var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(12) };
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            return t;
        }

        public static void Row(TableLayoutPanel t, string label, Control c)
        {
            int r = t.RowCount;
            t.RowCount = r + 1;
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 6, 12, 6) }, 0, r);
            c.Dock = DockStyle.Fill;
            c.Margin = new Padding(0, 3, 0, 3);
            t.Controls.Add(c, 1, r);
        }

        public static void FullRow(TableLayoutPanel t, Control c)
        {
            int r = t.RowCount;
            t.RowCount = r + 1;
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.Controls.Add(c, 0, r);
            t.SetColumnSpan(c, 2);
        }

        public static FlowLayoutPanel Buttons(params Button[] buttons)
        {
            var f = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.Right, WrapContents = false };
            foreach (var b in buttons) { b.AutoSize = true; b.Padding = new Padding(8, 2, 8, 2); f.Controls.Add(b); }
            return f;
        }

        public static Button Btn(string text) { return new Button { Text = text, AutoSize = true }; }
    }

    // ---------------------------------------------------------------- UC02
    public class LoginForm : Form
    {
        readonly TextBox txtUser = new TextBox();
        readonly TextBox txtPass = new TextBox { UseSystemPasswordChar = true };

        public LoginForm()
        {
            Text = "Đăng nhập";
            ClientSize = new Size(380, 190);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            var t = Ui.Form2Col();
            Ui.Row(t, "Tên đăng nhập:", txtUser);
            Ui.Row(t, "Mật khẩu:", txtPass);

            var btnLogin = Ui.Btn("Đăng nhập");
            var btnReg = Ui.Btn("Đăng ký tài khoản mới...");
            var btnCancel = Ui.Btn("Hủy");
            Ui.FullRow(t, Ui.Buttons(btnLogin, btnReg, btnCancel));
            Controls.Add(t);

            AcceptButton = btnLogin;
            CancelButton = btnCancel;

            btnLogin.Click += (s, e) =>
            {
                Customer c = null;
                if (Ui.Try(() => c = AppServices.Accounts.Login(txtUser.Text, txtPass.Text)))
                {
                    AppServices.CurrentCustomer = c;
                    DialogResult = DialogResult.OK;
                }
            };
            btnReg.Click += (s, e) =>
            {
                using (var f = new RegisterForm())
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        txtUser.Text = f.RegisteredUsername;
                        txtPass.Clear();
                        txtPass.Focus();
                    }
            };
        }
    }

    // ---------------------------------------------------------------- UC01
    public class RegisterForm : Form
    {
        readonly TextBox txtName = new TextBox(), txtId = new TextBox(), txtAddr = new TextBox(),
                         txtPhone = new TextBox(), txtUser = new TextBox(), txtEmail = new TextBox();
        readonly TextBox txtPass = new TextBox { UseSystemPasswordChar = true };
        readonly DateTimePicker dtpBirth = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = new DateTime(2000, 1, 1) };

        public string RegisteredUsername { get; private set; }

        public RegisterForm()
        {
            Text = "Đăng ký tài khoản khách hàng";
            ClientSize = new Size(460, 400);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            var t = Ui.Form2Col();
            Ui.Row(t, "Họ tên:", txtName);
            Ui.Row(t, "Ngày sinh:", dtpBirth);
            Ui.Row(t, "Số CMND/Passport:", txtId);
            Ui.Row(t, "Địa chỉ:", txtAddr);
            Ui.Row(t, "Điện thoại:", txtPhone);
            Ui.Row(t, "Tên đăng nhập:", txtUser);
            Ui.Row(t, "Mật khẩu:", txtPass);
            Ui.Row(t, "Email (không bắt buộc):", txtEmail);

            var btnOk = Ui.Btn("Đăng ký");
            var btnCancel = Ui.Btn("Hủy");
            Ui.FullRow(t, Ui.Buttons(btnOk, btnCancel));
            Controls.Add(t);
            AcceptButton = btnOk; CancelButton = btnCancel;

            btnOk.Click += (s, e) =>
            {
                var c = new Customer
                {
                    FullName = txtName.Text.Trim(), BirthDate = dtpBirth.Value, IdNumber = txtId.Text.Trim(),
                    Address = txtAddr.Text.Trim(), Phone = txtPhone.Text.Trim(),
                    Username = txtUser.Text, Email = txtEmail.Text
                };
                if (Ui.Try(() => AppServices.Accounts.Register(c, txtPass.Text)))
                {
                    RegisteredUsername = c.Username;
                    Ui.Info("Đăng ký thành công. Bạn có thể đăng nhập ngay.");
                    DialogResult = DialogResult.OK;
                }
            };
        }
    }

    // ---------------------------------------------------------------- UC04 (+ UC05 từ màn hình chi tiết)
    public class ProductDetailForm : Form
    {
        public ProductDetailForm(Product p)
        {
            Text = "Chi tiết sản phẩm";
            ClientSize = new Size(480, 400);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            var txt = new TextBox
            {
                Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill,
                Text = string.Join(Environment.NewLine, new[]
                {
                    "Mã sản phẩm   : " + p.Id,
                    "Tên sản phẩm  : " + p.Name,
                    "Nhà sản xuất  : " + p.Manufacturer,
                    "Nhóm sản phẩm : " + p.Group,
                    "Giá bán       : " + Fmt.Money(p.Price),
                    "Tình trạng    : " + (p.InStock ? "Còn hàng" : "Hết hàng"),
                    "",
                    "Mô tả:",
                    p.Description,
                    "",
                    "Thông số kỹ thuật:",
                    p.Specs,
                    "",
                    "Hình ảnh minh họa: " + p.Images
                })
            };

            var nud = new NumericUpDown { Minimum = 1, Maximum = 99, Value = 1, Width = 60 };
            var btnAdd = Ui.Btn("Thêm vào giỏ");
            var btnClose = Ui.Btn("Đóng");
            btnAdd.Enabled = p.InStock;
            btnAdd.Click += (s, e) =>
            {
                if (Ui.Try(() => AppServices.Cart.Add(p, (int)nud.Value)))
                {
                    Ui.Info("Đã thêm vào giỏ hàng.");
                    DialogResult = DialogResult.OK;
                }
            };
            btnClose.Click += (s, e) => DialogResult = DialogResult.Cancel;

            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(10) };
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var bottom = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.Right, WrapContents = false };
            bottom.Controls.Add(new Label { Text = "Số lượng:", AutoSize = true, Margin = new Padding(0, 8, 4, 0) });
            bottom.Controls.Add(nud);
            bottom.Controls.Add(btnAdd);
            bottom.Controls.Add(btnClose);
            root.Controls.Add(txt, 0, 0);
            root.Controls.Add(bottom, 0, 1);
            Controls.Add(root);
        }
    }

    // ---------------------------------------------------------------- UC08
    public class CheckoutForm : Form
    {
        readonly TextBox txtRName = new TextBox(), txtRAddr = new TextBox(), txtRPhone = new TextBox();
        readonly CheckBox chkSelf = new CheckBox { Text = "Người nhận là chính tôi", AutoSize = true };
        readonly ComboBox cboRegion = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        readonly ComboBox cboShip = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        readonly ComboBox cboCard = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        readonly TextBox txtCardNo = new TextBox { MaxLength = 16 };
        readonly NumericUpDown nudMonth = new NumericUpDown { Minimum = 1, Maximum = 12, Value = DateTime.Today.Month, Width = 50 };
        readonly NumericUpDown nudYear = new NumericUpDown { Minimum = DateTime.Today.Year, Maximum = DateTime.Today.Year + 15, Value = DateTime.Today.Year, Width = 70 };
        readonly TextBox txtHolder = new TextBox();
        readonly TextBox txtCsv = new TextBox { MaxLength = 4, UseSystemPasswordChar = true };
        readonly Label lblSub = new Label { AutoSize = true }, lblShip = new Label { AutoSize = true },
                       lblCardFee = new Label { AutoSize = true }, lblTotal = new Label { AutoSize = true, Font = new Font("Segoe UI", 11f, FontStyle.Bold) };
        readonly Label lblHint = new Label { AutoSize = true, ForeColor = Color.DimGray };
        bool _ready;

        public CheckoutForm()
        {
            Text = "Đặt hàng và tính tiền";
            ClientSize = new Size(560, 640);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            AutoScroll = true;

            var t = Ui.Form2Col();
            Ui.FullRow(t, new Label { Text = "1. Thông tin người nhận hàng", Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), AutoSize = true });
            Ui.Row(t, "", chkSelf);
            Ui.Row(t, "Họ tên:", txtRName);
            Ui.Row(t, "Địa chỉ:", txtRAddr);
            Ui.Row(t, "Điện thoại:", txtRPhone);

            Ui.FullRow(t, new Label { Text = "2. Hình thức giao hàng", Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 0) });
            Ui.Row(t, "Khu vực:", cboRegion);
            Ui.Row(t, "Loại phiếu đặt hàng:", cboShip);
            Ui.FullRow(t, lblHint);

            Ui.FullRow(t, new Label { Text = "3. Thanh toán bằng thẻ tín dụng", Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 0) });
            Ui.Row(t, "Loại thẻ:", cboCard);
            Ui.Row(t, "Số thẻ:", txtCardNo);
            var exp = new FlowLayoutPanel { AutoSize = true, WrapContents = false };
            exp.Controls.Add(new Label { Text = "Tháng", AutoSize = true, Margin = new Padding(0, 6, 4, 0) });
            exp.Controls.Add(nudMonth);
            exp.Controls.Add(new Label { Text = "Năm", AutoSize = true, Margin = new Padding(12, 6, 4, 0) });
            exp.Controls.Add(nudYear);
            Ui.Row(t, "Ngày hết hạn:", exp);
            Ui.Row(t, "Họ tên chủ thẻ:", txtHolder);
            Ui.Row(t, "Mã an ninh (CSV):", txtCsv);

            Ui.FullRow(t, new Label { Text = "4. Tổng kết", Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 0) });
            Ui.Row(t, "Tiền hàng:", lblSub);
            Ui.Row(t, "Phí giao hàng:", lblShip);
            Ui.Row(t, "Lệ phí thẻ:", lblCardFee);
            Ui.Row(t, "TỔNG CỘNG:", lblTotal);

            var btnOk = Ui.Btn("Đặt hàng");
            var btnCancel = Ui.Btn("Hủy");
            Ui.FullRow(t, Ui.Buttons(btnOk, btnCancel));
            Controls.Add(t);
            CancelButton = btnCancel;

            // nạp dữ liệu tra cứu từ CSDL
            _ready = false;
            Ui.Try(() =>
            {
                cboRegion.DataSource = new List<string>(AppServices.Lookup.GetRegions());
                cboCard.DataSource = new List<CardType>(AppServices.Lookup.GetCardTypes());
                cboCard.DisplayMember = "Name";
            });
            cboShip.DataSource = new List<ShipOption>
            {
                new ShipOption { Type = ShippingType.Normal, Name = Fmt.ShipName(ShippingType.Normal) },
                new ShipOption { Type = ShippingType.Express, Name = Fmt.ShipName(ShippingType.Express) },
                new ShipOption { Type = ShippingType.SameDay, Name = Fmt.ShipName(ShippingType.SameDay) }
            };
            cboShip.DisplayMember = "Name";
            _ready = true;

            lblHint.Text = "Miễn phí chuyển phát nhanh từ " + Fmt.Money(ShippingCalculator.FreeExpressFrom) +
                           "; chuyển phát nhanh trong ngày từ " + Fmt.Money(ShippingCalculator.FreeSameDayFrom) + ".";

            chkSelf.CheckedChanged += (s, e) =>
            {
                var c = AppServices.CurrentCustomer;
                if (chkSelf.Checked && c != null) { txtRName.Text = c.FullName; txtRAddr.Text = c.Address; txtRPhone.Text = c.Phone; }
                else if (!chkSelf.Checked) { txtRName.Clear(); txtRAddr.Clear(); txtRPhone.Clear(); }
            };
            cboRegion.SelectedIndexChanged += (s, e) => UpdateQuote();
            cboShip.SelectedIndexChanged += (s, e) => UpdateQuote();
            cboCard.SelectedIndexChanged += (s, e) =>
            {
                var ct = cboCard.SelectedItem as CardType;
                if (ct != null) { txtCardNo.MaxLength = ct.NumberLength; txtCsv.MaxLength = ct.CsvLength; }
                UpdateQuote();
            };
            btnOk.Click += (s, e) => PlaceOrder();
            UpdateQuote();
        }

        ShippingType CurrentType { get { var o = cboShip.SelectedItem as ShipOption; return o == null ? ShippingType.Normal : o.Type; } }

        void UpdateQuote()
        {
            if (!_ready) return;
            var ct = cboCard.SelectedItem as CardType;
            string region = cboRegion.SelectedItem as string;
            if (region == null || ct == null) return;
            try
            {
                Quote q = AppServices.Orders.GetQuote(AppServices.Cart.Subtotal, CurrentType, region, ct.Code);
                lblSub.Text = Fmt.Money(q.Subtotal);
                lblShip.Text = q.ShippingFee == 0 ? "Miễn phí" : Fmt.Money(q.ShippingFee);
                lblCardFee.Text = Fmt.Money(q.CardFee);
                lblTotal.Text = Fmt.Money(q.Total);
            }
            catch (Exception ex)
            {
                lblTotal.Text = "-";
                lblHint.Text = ex.Message;
            }
        }

        void PlaceOrder()
        {
            var ct = cboCard.SelectedItem as CardType;
            var receiver = new Receiver { FullName = txtRName.Text.Trim(), Address = txtRAddr.Text.Trim(), Phone = txtRPhone.Text.Trim() };
            var card = new CardInfo
            {
                CardTypeCode = ct == null ? null : ct.Code,
                Number = txtCardNo.Text.Trim(),
                ExpiryMonth = (int)nudMonth.Value,
                ExpiryYear = (int)nudYear.Value,
                HolderName = txtHolder.Text,
                Csv = txtCsv.Text.Trim()
            };

            Order order = null;
            // Nếu thẻ sai / bị từ chối: hiện thông báo và giữ nguyên form để khách nhập lại thẻ
            if (Ui.Try(() => order = AppServices.Orders.PlaceOrder(
                    AppServices.CurrentCustomer, receiver, CurrentType, cboRegion.SelectedItem as string, card, AppServices.Cart)))
            {
                string msg = "Đặt hàng thành công!\nMã đơn hàng: " + order.OrderId + "\nTổng thanh toán: " + Fmt.Money(order.Total);
                if (order.EmailSent) msg += "\nEmail xác nhận đã được gửi tới " + AppServices.CurrentCustomer.Email + ".";
                else if (!string.IsNullOrWhiteSpace(AppServices.CurrentCustomer.Email)) msg += "\n(Chưa gửi được email xác nhận.)";
                Ui.Info(msg);
                DialogResult = DialogResult.OK;
            }
        }
    }

    // ---------------------------------------------------------------- Màn hình chính: UC03, UC05, UC06, UC07
    public class MainForm : Form
    {
        readonly Label lblUser = new Label { AutoSize = true, Margin = new Padding(0, 8, 12, 0) };
        readonly Button btnLogin = Ui.Btn("Đăng nhập"), btnRegister = Ui.Btn("Đăng ký"), btnLogout = Ui.Btn("Đăng xuất");
        readonly ComboBox cboGroup = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
        readonly ListView lvProducts = NewList(), lvCart = NewList();
        readonly NumericUpDown nudQty = new NumericUpDown { Minimum = 1, Maximum = 99, Value = 1, Width = 55 };
        readonly NumericUpDown nudCartQty = new NumericUpDown { Minimum = 1, Maximum = 99, Value = 1, Width = 55 };
        readonly Label lblTotal = new Label { AutoSize = true, Font = new Font("Segoe UI", 10.5f, FontStyle.Bold) };

        static ListView NewList()
        {
            return new ListView { View = View.Details, FullRowSelect = true, GridLines = true, MultiSelect = false, Dock = DockStyle.Fill, HideSelection = false };
        }

        public MainForm()
        {
            Text = "e-SHOPPING - Cửa hàng ABC";
            ClientSize = new Size(1120, 640);
            StartPosition = FormStartPosition.CenterScreen;
            BuildUi();
            Load += (s, e) => { LoadGroups(); RefreshUser(); RefreshCart(); };
        }

        void BuildUi()
        {
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var top = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(8) };
            top.Controls.AddRange(new Control[] { lblUser, btnLogin, btnRegister, btnLogout });
            root.Controls.Add(top, 0, 0);

            var split = new SplitContainer { Dock = DockStyle.Fill };
            Load += (s, e) => { try { split.SplitterDistance = 640; } catch { } };

            // ----- bên trái: sản phẩm -----
            lvProducts.Columns.Add("Mã", 70);
            lvProducts.Columns.Add("Tên sản phẩm", 190);
            lvProducts.Columns.Add("Nhà SX", 90);
            lvProducts.Columns.Add("Giá bán", 100);
            lvProducts.Columns.Add("Tình trạng", 80);

            var left = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3, Padding = new Padding(6) };
            left.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            left.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            left.Controls.Add(new Label { Text = "Nhóm sản phẩm:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
            left.Controls.Add(cboGroup, 1, 0);
            left.Controls.Add(lvProducts, 0, 1);
            left.SetColumnSpan(lvProducts, 2);

            var btnDetail = Ui.Btn("Xem chi tiết");
            var btnAdd = Ui.Btn("Thêm vào giỏ");
            var pbar = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.Left, WrapContents = false };
            pbar.Controls.Add(new Label { Text = "Số lượng:", AutoSize = true, Margin = new Padding(0, 8, 4, 0) });
            pbar.Controls.Add(nudQty);
            pbar.Controls.Add(btnAdd);
            pbar.Controls.Add(btnDetail);
            left.Controls.Add(pbar, 0, 2);
            left.SetColumnSpan(pbar, 2);
            split.Panel1.Controls.Add(left);

            // ----- bên phải: giỏ hàng -----
            lvCart.Columns.Add("Sản phẩm", 190);
            lvCart.Columns.Add("SL", 40);
            lvCart.Columns.Add("Đơn giá", 90);
            lvCart.Columns.Add("Thành tiền", 100);

            var right = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(6) };
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            right.Controls.Add(lvCart, 0, 0);

            var btnUpdate = Ui.Btn("Cập nhật số lượng");
            var btnRemove = Ui.Btn("Loại bỏ khỏi giỏ");
            var cbar = new FlowLayoutPanel { AutoSize = true, WrapContents = false };
            cbar.Controls.Add(new Label { Text = "Số lượng:", AutoSize = true, Margin = new Padding(0, 8, 4, 0) });
            cbar.Controls.Add(nudCartQty);
            cbar.Controls.Add(btnUpdate);
            cbar.Controls.Add(btnRemove);
            right.Controls.Add(cbar, 0, 1);
            right.Controls.Add(lblTotal, 0, 2);

            var btnCheckout = new Button { Text = "Tính tiền / Đặt hàng", AutoSize = true, Padding = new Padding(10, 4, 10, 4), Anchor = AnchorStyles.Right };
            right.Controls.Add(btnCheckout, 0, 3);
            split.Panel2.Controls.Add(right);

            root.Controls.Add(split, 0, 1);
            Controls.Add(root);

            // ----- sự kiện -----
            cboGroup.SelectedIndexChanged += (s, e) => LoadProducts();
            lvProducts.DoubleClick += (s, e) => ShowDetail();
            btnDetail.Click += (s, e) => ShowDetail();
            btnAdd.Click += (s, e) => AddSelected();
            lvCart.SelectedIndexChanged += (s, e) =>
            {
                if (lvCart.SelectedItems.Count == 0) return;
                var it = FindCartItem((string)lvCart.SelectedItems[0].Tag);
                if (it != null) nudCartQty.Value = Math.Min(99, Math.Max(1, it.Quantity));
            };
            btnUpdate.Click += (s, e) =>
            {
                if (lvCart.SelectedItems.Count == 0) { Ui.Error("Hãy chọn một sản phẩm trong giỏ."); return; }
                if (Ui.Try(() => AppServices.Cart.UpdateQuantity((string)lvCart.SelectedItems[0].Tag, (int)nudCartQty.Value))) RefreshCart();
            };
            btnRemove.Click += (s, e) =>
            {
                if (lvCart.SelectedItems.Count == 0) { Ui.Error("Hãy chọn một sản phẩm trong giỏ."); return; }
                AppServices.Cart.Remove((string)lvCart.SelectedItems[0].Tag);
                RefreshCart();
            };
            btnCheckout.Click += (s, e) => Checkout();
            btnLogin.Click += (s, e) => { using (var f = new LoginForm()) f.ShowDialog(this); RefreshUser(); };
            btnRegister.Click += (s, e) => { using (var f = new RegisterForm()) f.ShowDialog(this); };
            btnLogout.Click += (s, e) => { AppServices.CurrentCustomer = null; RefreshUser(); };
        }

        static CartItem FindCartItem(string id)
        {
            foreach (var i in AppServices.Cart.Items) if (i.Product.Id == id) return i;
            return null;
        }

        void LoadGroups()
        {
            cboGroup.DataSource = new List<string>(AppServices.Products.GetGroups());
        }

        void LoadProducts()
        {
            lvProducts.Items.Clear();
            string g = cboGroup.SelectedItem as string;
            if (g == null) return;
            foreach (var p in AppServices.Products.GetProductsByGroup(g))
            {
                var item = new ListViewItem(new[] { p.Id, p.Name, p.Manufacturer, Fmt.Money(p.Price), p.InStock ? "Còn hàng" : "Hết hàng" });
                item.Tag = p;
                if (!p.InStock) item.ForeColor = Color.Gray;
                lvProducts.Items.Add(item);
            }
        }

        Product SelectedProduct()
        {
            if (lvProducts.SelectedItems.Count == 0) { Ui.Error("Hãy chọn một sản phẩm trong danh sách."); return null; }
            return (Product)lvProducts.SelectedItems[0].Tag;
        }

        void ShowDetail()
        {
            var p = SelectedProduct();
            if (p == null) return;
            using (var f = new ProductDetailForm(AppServices.Products.GetProduct(p.Id) ?? p))
                if (f.ShowDialog(this) == DialogResult.OK) RefreshCart();
        }

        void AddSelected()
        {
            var p = SelectedProduct();
            if (p == null) return;
            if (Ui.Try(() => AppServices.Cart.Add(p, (int)nudQty.Value))) RefreshCart();
        }

        void RefreshCart()
        {
            lvCart.Items.Clear();
            foreach (var i in AppServices.Cart.Items)
            {
                var item = new ListViewItem(new[] { i.Product.Name, i.Quantity.ToString(), Fmt.Money(i.Product.Price), Fmt.Money(i.LineTotal) });
                item.Tag = i.Product.Id;
                lvCart.Items.Add(item);
            }
            lblTotal.Text = "Tạm tính: " + Fmt.Money(AppServices.Cart.Subtotal);
        }

        void RefreshUser()
        {
            var c = AppServices.CurrentCustomer;
            lblUser.Text = c == null ? "Bạn chưa đăng nhập" : "Xin chào, " + c.FullName;
            btnLogin.Visible = btnRegister.Visible = c == null;
            btnLogout.Visible = c != null;
        }

        void Checkout()
        {
            if (AppServices.Cart.IsEmpty) { Ui.Error("Giỏ hàng đang trống."); return; }
            if (AppServices.CurrentCustomer == null)
            {
                using (var f = new LoginForm())
                    if (f.ShowDialog(this) != DialogResult.OK) return;
                RefreshUser();
            }
            using (var f = new CheckoutForm())
                if (f.ShowDialog(this) == DialogResult.OK) RefreshCart();
        }
    }
}
