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
using heqtcsdl.Model;

namespace heqtcsdl.View
{
    public partial class CartForm : Form
    {
        // BƯỚC 2: Khai báo và khởi tạo Controller
        private readonly ProductController _controller = new ProductController();

        public CartForm()
        {
            InitializeComponent();
            // Tải giỏ hàng khi Form mở
            this.Load += CartForm_Load;
        }

        private void CartForm_Load(object sender, EventArgs e)
        {
            DisplayCart();
        }

        // BƯỚC 3: Phương thức chính hiển thị giỏ hàng
        private void DisplayCart()
        {
            var cartItems = _controller.GetCartItems();

            // Binding list vào DataGridView
            // Dùng ToList() để tạo bản sao và binding dễ dàng hơn
            dgvCartItems.DataSource = cartItems.ToList();

            // Tính tổng tiền
            decimal totalPrice = cartItems.Sum(item => item.ThanhTien);
            // LƯU Ý: Phải đặt tên lblTotalPrice chính xác để code này chạy
            lblTotalPrice.Text = $"Tổng Cộng: {totalPrice:N0} VNĐ";

            // Ẩn cột không cần thiết
            if (dgvCartItems.Columns.Contains("MaSanPham"))
            {
                dgvCartItems.Columns["MaSanPham"].Visible = false;
            }
        }

        // BƯỚC 4a: Xử lý sự kiện khi nhấn nút Đặt hàng
        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (_controller.GetCartItems().Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống. Vui lòng chọn sản phẩm để đặt hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi Controller để thực hiện giao dịch CSDL (usp_TaoDonHang)
            string resultMessage = _controller.PlaceOrder();

            MessageBox.Show(resultMessage, "Kết Quả Đặt Hàng", MessageBoxButtons.OK,
                resultMessage.StartsWith("Lỗi") ? MessageBoxIcon.Error : MessageBoxIcon.Information);

            if (!resultMessage.StartsWith("Lỗi"))
            {
                // Nếu thành công, làm mới lại giỏ hàng (sẽ trống)
                DisplayCart();
                // Sau đó, đóng Form giỏ hàng để khách hàng tiếp tục mua sắm
                this.Close();
            }
        }

        // BƯỚC 4b: Xử lý sự kiện khi nhấn nút Xóa Giỏ hàng
        private void btnClearCart_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa tất cả sản phẩm khỏi giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _controller.ClearCart();
                DisplayCart();
                MessageBox.Show("Giỏ hàng đã được làm trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // BƯỚC 4c: Xử lý sự kiện khi nhấn nút Tiếp tục mua sắm
        private void btnContinueShopping_Click(object sender, EventArgs e)
        {
            // Đơn giản là đóng Form này lại
            this.Close();
        }

        // Phương thức khác: DgvCellContentClick (Giữ nguyên)
        private void dgvCartItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Có thể thêm logic xóa từng sản phẩm ở đây nếu bạn thêm nút vào DGV
        }

        private void lblTotalPrice_Click(object sender, EventArgs e) { /* Không cần logic */ }
    }
}