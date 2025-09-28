using heqtcsdl.Controller;
using heqtcsdl.Model.heqtcsdl.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace heqtcsdl.View
{
    public partial class CustomerDashboard : Form
    {
        private readonly CustomerController _controller;

        // Khai báo các Controls cần thiết (Tương tự như file Designer)
        private GroupBox grpInfo;
        private DataGridView dgvOrderHistory;

        // Các Label để hiển thị dữ liệu
        private Label lblCustomerName;
        private Label lblPhoneNumber;
        private Label lblTotalPoints;
        private Label lblMembershipRank;


        public CustomerDashboard()
        {
            // Tự động gọi InitializeComponent để khởi tạo giao diện
            InitializeComponent();
            _controller = new CustomerController();

            // Tải dữ liệu khi Form được hiển thị
            this.Load += CustomerDashboard_Load;
        }

        // Phương thức Khởi tạo Giao diện (Tương đương với file Designer)
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomerDashboard
            // 
            this.ClientSize = new System.Drawing.Size(1396, 575);
            this.Name = "CustomerDashboard";
            this.ResumeLayout(false);

        }

        private void CustomerDashboard_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        // --- CODE LOGIC CÒN LẠI KHÔNG THAY ĐỔI ---
        private void LoadCustomerData()
        {
            try
            {
                // Lấy thông tin cá nhân
                Customer customerInfo = _controller.LoadCustomerDashboardData();
                DataTable orderHistory = _controller.LoadOrderHistory();

                // Cập nhật UI (LABEL)
                if (customerInfo != null)
                {
                    lblCustomerName.Text = "Tên Khách hàng: " + customerInfo.FullName;
                    lblPhoneNumber.Text = "Số điện thoại: " + customerInfo.PhoneNumber;
                    lblTotalPoints.Text = "Tổng điểm tích lũy: " + customerInfo.TotalPoints.ToString("N0");
                    lblMembershipRank.Text = "Hạng thành viên: " + customerInfo.MembershipRank;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin khách hàng.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Cập nhật UI (DATAGRIDVIEW)
                dgvOrderHistory.DataSource = orderHistory;

                // Định dạng hiển thị
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
                MessageBox.Show($"Lỗi tải dữ liệu: Vui lòng kiểm tra kết nối SQL. {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}