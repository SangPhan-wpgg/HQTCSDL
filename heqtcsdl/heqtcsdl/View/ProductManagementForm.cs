using heqtcsdl.Controller;
using heqtcsdl.Model;
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
    public partial class ProductManagementForm : Form
    {
        private readonly ProductManagementController _controller = new ProductManagementController();
        public ProductManagementForm()
        {
            InitializeComponent();
            this.Load += ProductManagementForm_Load;
        }

        private void ProductManagementForm_Load(object sender, EventArgs e)
        {
            LoadProductData();
        }
        private void LoadProductData()
        {
            try
            {
                // Gọi Controller để lấy danh sách sản phẩm
                DataTable products = _controller.LoadAllProducts();
                dgvProducts.DataSource = products;

                // Định dạng hiển thị
                if (dgvProducts.Columns.Contains("Gia"))
                {
                    dgvProducts.Columns["Gia"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenSanPham.Text) || string.IsNullOrWhiteSpace(txtGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên và Giá.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtGia.Text, out decimal gia))
            {
                MessageBox.Show("Giá không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Product newProduct = new Product
            {
                TenSanPham = txtTenSanPham.Text,
                Gia = gia,
                // Giả sử các thuộc tính khác có thể được thiết lập ở đây
            };

            try
            {
                string result = _controller.HandleAddProduct(newProduct);
                MessageBox.Show(result, "Thêm sản phẩm", MessageBoxButtons.OK,
                    result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                // Tải lại dữ liệu sau khi thêm
                LoadProductData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có hàng nào đang được chọn không
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần cập nhật trong bảng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy MaSanPham từ hàng đang được chọn
            int maSanPham = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["MaSanPham"].Value);

            if (string.IsNullOrWhiteSpace(txtTenSanPham.Text) || !decimal.TryParse(txtGia.Text, out decimal gia))
            {
                MessageBox.Show("Dữ liệu cập nhật không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Product updatedProduct = new Product
            {
                // Quan trọng: Phải gửi MaSanPham lên để CSDL biết sửa bản ghi nào
                MaSanPham = maSanPham,
                TenSanPham = txtTenSanPham.Text,
                Gia = gia,
                // ... (các thuộc tính khác nếu có)
            };

            try
            {
                // Bạn cần thêm phương thức HandleUpdateProduct(Product product) vào ProductManagementController
                string result = _controller.HandleUpdateProduct(updatedProduct);
                MessageBox.Show(result, "Cập nhật sản phẩm", MessageBoxButtons.OK,
                    result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                LoadProductData(); // Tải lại dữ liệu
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa trong bảng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maSanPham = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["MaSanPham"].Value);
            string tenSanPham = dgvProducts.SelectedRows[0].Cells["TenSanPham"].Value.ToString();

            // Xác nhận xóa
            DialogResult confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{tenSanPham}' (Mã: {maSanPham}) không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    // Bạn cần thêm phương thức HandleDeleteProduct(int maSanPham) vào ProductManagementController
                    string result = _controller.HandleDeleteProduct(maSanPham);
                    MessageBox.Show(result, "Xóa sản phẩm", MessageBoxButtons.OK,
                        result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                    LoadProductData(); // Tải lại dữ liệu
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtTenSanPham_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtGia_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
                // Hiển thị dữ liệu lên các TextBox để chỉnh sửa
                txtTenSanPham.Text = row.Cells["TenSanPham"].Value.ToString();
                txtGia.Text = row.Cells["Gia"].Value.ToString();
            }
        }
    }
}
