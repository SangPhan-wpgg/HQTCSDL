using heqtcsdl.Controller;
using heqtcsdl.Model;
using heqtcsdl.Model.heqtcsdl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace heqtcsdl
{

    public partial class CustomerAccountManagementForm : Form
    {
        private readonly CustomerManagementController _controller = new CustomerManagementController();
        private int _selectedCustomerId = -1;
        private Timer _searchTimer = new Timer();

        public CustomerAccountManagementForm()
        {
            InitializeComponent();
            this.Load += CustomerAccountManagementForm_Load;

            // Gán sự kiện
            dgvCustomers.CellClick += dgvCustomers_CellClick;
            btnAdd.Click += btnAdd_Click;
            btnUpdateInfo.Click += btnUpdateInfo_Click;
            btnDelete.Click += btnDelete_Click;

            // Thiết lập Tìm kiếm
            if (txtSearchCustomer != null)
            {
                txtSearchCustomer.TextChanged += txtSearchCustomer_TextChanged;
            }
            _searchTimer.Interval = 500;
            _searchTimer.Tick += SearchTimer_Tick;
        }


        private void CustomerAccountManagementForm_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            try
            {
                DataTable customers = _controller.LoadAllCustomers();
                dgvCustomers.DataSource = customers;

                ClearInputFields();
                _selectedCustomerId = -1;
                this.Text = "Quản lý Tài khoản Khách hàng"; // Đặt lại tiêu đề Form
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputFields()
        {
            // LOẠI BỎ txtMaThanhVien.Text = "";
            txtTen.Text = "";
            txtEmail.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";

            if (txtUsername != null) txtUsername.Text = "";
            if (txtPassword != null) txtPassword.Text = "";
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        
        // Xử lý khi click vào một hàng trong DataGridView
        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];

                _selectedCustomerId = Convert.ToInt32(row.Cells["MaThanhVien"].Value);
                string selectedCustomerName = row.Cells["TenThanhVien"].Value.ToString();

                // Loại bỏ txtMaThanhVien
                txtTen.Text = selectedCustomerName;
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtSDT.Text = row.Cells["SDT"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();

                // Cập nhật tiêu đề Form để hiển thị ID đang chọn
                this.Text = $"Quản lý Tài khoản Khách hàng | Đang chọn: #{_selectedCustomerId} ({selectedCustomerName})";

                // Ẩn/Xóa dữ liệu nhạy cảm
                if (txtUsername != null) txtUsername.Text = "";
                if (txtPassword != null) txtPassword.Text = "";
            }
        }

        private void txtTen_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdateInfo_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId == -1 || string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng và nhập Tên/Email hợp lệ để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Customer updatedCustomer = new Customer
            {
                MaKhachHang = _selectedCustomerId,
                HoTen = txtTen.Text,
                Email = txtEmail.Text,
                SoDienThoai = txtSDT.Text,
                DiaChi = txtDiaChi.Text
            };

            try
            {
                string result = _controller.HandleUpdateCustomer(updatedCustomer);
                MessageBox.Show(result, "Cập nhật Thông tin", MessageBoxButtons.OK,
                    result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                LoadCustomerData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];

                _selectedCustomerId = Convert.ToInt32(row.Cells["MaThanhVien"].Value);
                string selectedCustomerName = row.Cells["TenThanhVien"].Value.ToString();

                // Loại bỏ txtMaThanhVien
                txtTen.Text = selectedCustomerName;
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtSDT.Text = row.Cells["SDT"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();

                // Cập nhật tiêu đề Form để hiển thị ID đang chọn
                this.Text = $"Quản lý Tài khoản Khách hàng | Đang chọn: #{_selectedCustomerId} ({selectedCustomerName})";

                // Ẩn/Xóa dữ liệu nhạy cảm
                if (txtUsername != null) txtUsername.Text = "";
                if (txtPassword != null) txtPassword.Text = "";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên, Email, Username và Mật khẩu để thêm khách hàng.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashedPassword = HashPassword(txtPassword.Text);

            Customer newCustomer = new Customer
            {
                HoTen = txtTen.Text,
                Email = txtEmail.Text,
                SoDienThoai = txtSDT.Text,
                DiaChi = txtDiaChi.Text
            };

            try
            {
                string result = _controller.HandleAddCustomer(newCustomer, txtUsername.Text, hashedPassword);
                MessageBox.Show(result, "Thêm Khách hàng", MessageBoxButtons.OK,
                    result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                LoadCustomerData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId == -1)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy tên khách hàng từ hàng đang chọn
            string tenKhachHang = dgvCustomers.SelectedRows.Count > 0 ? dgvCustomers.SelectedRows[0].Cells["TenThanhVien"].Value.ToString() : "khách hàng này";

            DialogResult confirm = MessageBox.Show($"Bạn có chắc chắn muốn XÓA vĩnh viễn tài khoản của {tenKhachHang} (ID: {_selectedCustomerId}) không?",
                "Xác nhận XÓA TÀI KHOẢN", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    string result = _controller.HandleDeleteCustomer(_selectedCustomerId);

                    MessageBox.Show(result, "Xóa Tài khoản", MessageBoxButtons.OK,
                        result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                    LoadCustomerData();
                    ClearInputFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            ExecuteCustomerSearch();
        }
        private void ExecuteCustomerSearch()
        {
            string searchTerm = txtSearchCustomer.Text.Trim();

            try
            {
                // Dùng Controller đã được cập nhật để xử lý tìm kiếm
                DataTable searchResults = _controller.ExecuteSearch(searchTerm);
                dgvCustomers.DataSource = searchResults;

                ClearInputFields();
                _selectedCustomerId = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtSearchCustomer_TextChanged(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }
    }
}
