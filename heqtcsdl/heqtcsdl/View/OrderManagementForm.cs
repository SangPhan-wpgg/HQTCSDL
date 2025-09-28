using heqtcsdl.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace heqtcsdl.View
{
    public partial class OrderManagementForm : Form
    {
        // Khai báo và khởi tạo Controller
        private readonly OrderManagementController _controller = new OrderManagementController();
        private int _selectedOrderId = -1; // Biến để lưu ID đơn hàng đang được chọn

        public OrderManagementForm()
        {
            InitializeComponent();
            this.Load += OrderManagementForm_Load;

            // Gán sự kiện cho các controls
            // Dùng CellClick thay vì CellContentClick để bắt click vào bất kỳ ô nào
            dgvOrders.CellClick += dgvOrders_CellClick;
        }

        private void OrderManagementForm_Load(object sender, EventArgs e)
        {
            // Tải dữ liệu đơn hàng khi Form mở
            LoadOrderData();

            // Khởi tạo ComboBox trạng thái
            InitializeStatusComboBox();
        }

        private void InitializeStatusComboBox()
        {
            // Các trạng thái đơn hàng khả dụng (phải khớp với logic CSDL của bạn)
            cmbStatus.Items.Add("Chờ xử lý");
            cmbStatus.Items.Add("Đang giao");
            cmbStatus.Items.Add("Đã hoàn thành");
            cmbStatus.Items.Add("Đã hủy");
            // Không chọn mặc định, để người dùng phải chọn rõ ràng khi cập nhật
        }

        private void LoadOrderData()
        {
            try
            {
                DataTable orders = _controller.LoadAllOrders();
                dgvOrders.DataSource = orders;

                // Định dạng hiển thị
                if (dgvOrders.Columns.Contains("TongTien"))
                {
                    dgvOrders.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                    dgvOrders.Columns["TongTien"].HeaderText = "Tổng Tiền (VNĐ)";
                }

                // Đặt lại trạng thái lựa chọn
                _selectedOrderId = -1;
                lblSelectedOrder.Text = "Chưa chọn đơn hàng"; // Dùng lblSelectedOrder nếu bạn không dùng lblSelectedOrderId
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu đơn hàng: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức xử lý sự kiện khi chọn một hàng trong DataGridView
        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvOrders.Rows[e.RowIndex];

                // Lấy Mã Đơn hàng và trạng thái hiện tại
                _selectedOrderId = Convert.ToInt32(row.Cells["MaDonHang"].Value);

                // Trạng thái hiện tại
                if (row.Cells["TrangThai"]?.Value != null)
                {
                    string currentStatus = row.Cells["TrangThai"].Value.ToString();

                    // Hiển thị thông tin
                    lblSelectedOrder.Text = $"Đơn hàng đang chọn: #{_selectedOrderId} (TT Hiện tại: {currentStatus})";

                    // Đặt ComboBox về trạng thái hiện tại của đơn hàng
                    cmbStatus.SelectedItem = currentStatus;
                }
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút Cập nhật Trạng thái
        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (_selectedOrderId == -1)
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để cập nhật trạng thái.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn trạng thái mới.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newStatus = cmbStatus.SelectedItem.ToString();

            // Xác nhận cập nhật
            DialogResult confirm = MessageBox.Show($"Bạn có chắc chắn muốn cập nhật trạng thái đơn hàng #{_selectedOrderId} thành '{newStatus}' không?",
                "Xác nhận cập nhật", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    // Gọi Controller để thực thi SP (usp_UpdateOrderStatus)
                    string result = _controller.HandleUpdateStatus(_selectedOrderId, newStatus);

                    MessageBox.Show(result, "Cập nhật Trạng thái", MessageBoxButtons.OK,
                        result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                    // Tải lại dữ liệu sau khi cập nhật
                    LoadOrderData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- Các phương thức tự động sinh (Giữ nguyên hoặc xóa nếu không dùng) ---
        private void lblSelectedOrder_Click(object sender, EventArgs e) { /* Không cần logic */ }
        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e) { /* Dùng dgvOrders_CellClick thay thế */ }
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e) { /* Không cần logic */ }
    }
}