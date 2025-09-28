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
    public partial class ProductListForm : Form
    {
        // Khai báo Controller
        private readonly ProductController _controller;

        public ProductListForm()
        {
            InitializeComponent();
            _controller = new ProductController(); // Khởi tạo Controller

            // Tải danh sách sản phẩm khi Form mở
            this.Load += ProductListForm_Load;
        }

        private void ProductListForm_Load(object sender, EventArgs e)
        {
            // Tải danh sách đầy đủ ban đầu
            LoadProductData(string.Empty);
        }

        // Phương thức chính để tải và hiển thị dữ liệu (có thể dùng cho cả tải ban đầu và tìm kiếm)
        private void LoadProductData(string searchTerm)
        {
            DataTable products;
            try
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    // Lấy toàn bộ danh sách
                    products = _controller.LoadProductList();
                }
                else
                {
                    // Thực hiện tìm kiếm (gọi usp_SearchProducts)
                    products = _controller.ExecuteSearch(searchTerm);
                }

                // Gán dữ liệu vào DataGridView
                dgvProductList.DataSource = products;

                // Định dạng hiển thị
                if (dgvProductList.Columns.Contains("Gia"))
                {
                    dgvProductList.Columns["Gia"].DefaultCellStyle.Format = "N0"; // Định dạng tiền tệ
                    dgvProductList.Columns["Gia"].HeaderText = "Giá Bán";
                }
                if (dgvProductList.Columns.Contains("TenSanPham"))
                {
                    dgvProductList.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
                }
                // ... (Các định dạng cột khác) ...
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải/tìm kiếm sản phẩm: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi nhấn nút Tìm kiếm
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Lấy từ khóa tìm kiếm từ TextBox
            string searchTerm = txtSearchTerm.Text.Trim();
            // Gọi phương thức tải dữ liệu với từ khóa tìm kiếm
            LoadProductData(searchTerm);
        }

        // --- Các phương thức tự động sinh khác giữ nguyên ---
        private void txtSearchTerm_TextChanged(object sender, EventArgs e)
        {
            // Bạn có thể thêm logic tìm kiếm ngay khi gõ ở đây (Tùy chọn)
        }

        private void dgvProductList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // BƯỚC 1: Kiểm tra xem có phải cột "Thêm vào Giỏ" VÀ có phải hàng dữ liệu hợp lệ không.
            if (dgvProductList.Columns[e.ColumnIndex].Name == "colAddToCart" && e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductList.Rows[e.RowIndex];

                // BƯỚC 2: Lấy dữ liệu sản phẩm từ hàng được chọn

                // Cần đảm bảo cột "MaSanPham" tồn tại và là kiểu int
                int maSanPham = Convert.ToInt32(row.Cells["MaSanPham"].Value);

                // Lấy Tên và Giá
                string tenSanPham = row.Cells["TenSanPham"].Value.ToString();
                decimal gia = Convert.ToDecimal(row.Cells["Gia"].Value);

                int soLuong = 1; // Mặc định Khách hàng thêm 1 sản phẩm

                try
                {
                    // BƯỚC 3: Gọi Controller để xử lý logic Giỏ hàng
                    _controller.AddToCart(maSanPham, tenSanPham, gia, soLuong);

                    // BƯỚC 4: Thông báo thành công cho người dùng
                    MessageBox.Show($"Đã thêm {tenSanPham} (x{soLuong}) vào giỏ hàng!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể thêm vào giỏ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnViewCart_Click(object sender, EventArgs e)
        {

        }
    }
}