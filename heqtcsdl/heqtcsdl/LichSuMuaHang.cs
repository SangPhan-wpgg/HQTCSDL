using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using heqtcsdl.Controller; 
using heqtcsdl.Model.heqtcsdl.Model;

namespace heqtcsdl
{
    // Đảm bảo Form vẫn kế thừa từ Form và là partial class
    public partial class LichSuMuaHang : Form
    {
        // BƯỚC 2: Khai báo và khởi tạo Controller
        private readonly CustomerController _controller;

        public LichSuMuaHang()
        {
            InitializeComponent();
            _controller = new CustomerController(); // Khởi tạo Controller

            // BƯỚC 4: Gọi phương thức tải dữ liệu khi Form được tải
            this.Load += LichSuMuaHang_Load;
        }

        private void LichSuMuaHang_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        // BƯỚC 3: Thêm phương thức tải dữ liệu
        private void LoadCustomerData()
        {
            try
            {
                // Lấy thông tin cá nhân và lịch sử đơn hàng
                Customer customerInfo = _controller.LoadCustomerDashboardData();
                DataTable orderHistory = _controller.LoadOrderHistory();

                // === CẬP NHẬT UI (THÔNG TIN CÁ NHÂN) ===
                if (customerInfo != null)
                {
                    lblCustomerName.Text = "Tên Khách hàng: " + customerInfo.FullName;
                    lblPhoneNumber.Text = "Số điện thoại: " + customerInfo.PhoneNumber;
                    // Định dạng số điểm
                    lblTotalPoints.Text = "Tổng điểm tích lũy: " + customerInfo.TotalPoints.ToString("N0");
                    lblMembershipRank.Text = "Hạng thành viên: " + customerInfo.MembershipRank;
                }
                else
                {
                    // Trường hợp không tìm thấy thông tin khách hàng (rất hiếm khi xảy ra)
                    MessageBox.Show("Không tìm thấy thông tin khách hàng.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // === CẬP NHẬT UI (LỊCH SỬ MUA HÀNG) ===
                dgvOrderHistory.DataSource = orderHistory;

                // Định dạng hiển thị DataGridView
                if (dgvOrderHistory.Columns.Contains("TongTien"))
                {
                    dgvOrderHistory.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                    dgvOrderHistory.Columns["TongTien"].HeaderText = "Tổng Tiền";
                }
                if (dgvOrderHistory.Columns.Contains("MaDonHang"))
                {
                    dgvOrderHistory.Columns["MaDonHang"].HeaderText = "Mã ĐH";
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: mất kết nối, lỗi phân quyền)
                MessageBox.Show($"Lỗi tải dữ liệu: Vui lòng kiểm tra kết nối SQL. {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- GIỮ NGUYÊN CÁC PHƯƠNG THỨC XỬ LÝ SỰ KIỆN TỰ ĐỘNG SINH DƯỚI ĐÂY ---
        private void lblTitle_Click(object sender, EventArgs e) { }
        private void lblCustomerName_Click(object sender, EventArgs e) { }
        private void lblPhoneNumber_Click(object sender, EventArgs e) { }
        private void lblTotalPoints_Click(object sender, EventArgs e) { }
        private void lblMembershipRank_Click(object sender, EventArgs e) { }
        private void dgvOrderHistory_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}