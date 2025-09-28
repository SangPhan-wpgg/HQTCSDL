using heqtcsdl.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using heqtcsdl.Model;
using heqtcsdl.Dao;



namespace heqtcsdl.Controller
{
    public class ProductController
    {
        private readonly ProductDAO _productDAO = new ProductDAO();
        private readonly TransactionDAO _transactionDAO = new TransactionDAO();

        // Dùng danh sách tĩnh (static) để lưu trữ Giỏ hàng trong suốt phiên làm việc
        private static List<CartItem> _currentCart = new List<CartItem>();

        public string PlaceOrder()
        {
            // 1. Lấy giỏ hàng hiện tại
            List<CartItem> cart = _currentCart;

            // 2. Gọi TransactionDAO để thực hiện giao dịch CSDL
            string result = _transactionDAO.CreateOrder(cart);

            // 3. Nếu thành công, xóa giỏ hàng khỏi bộ nhớ
            if (!result.StartsWith("Lỗi"))
            {
                ClearCart();
            }

            return result;
        }

        // Lấy danh sách sản phẩm (đã có)
        public DataTable LoadProductList()
        {
            return _productDAO.GetAllProducts();
        }

        // Thực thi tìm kiếm (đã có)
        public DataTable ExecuteSearch(string searchTerm)
        {
            return _productDAO.SearchProducts(searchTerm);
        }

        // THÊM: Logic quản lý Giỏ hàng

        public List<CartItem> GetCartItems()
        {
            return _currentCart;
        }

        public void AddToCart(int maSanPham, string tenSanPham, decimal gia, int soLuong)
        {
            var existingItem = _currentCart.FirstOrDefault(item => item.MaSanPham == maSanPham);

            if (existingItem != null)
            {
                // Nếu sản phẩm đã có, tăng số lượng
                existingItem.SoLuongMua += soLuong;
            }
            else
            {
                // Nếu sản phẩm chưa có, thêm mới
                _currentCart.Add(new CartItem
                {
                    MaSanPham = maSanPham,
                    TenSanPham = tenSanPham,
                    Gia = gia,
                    SoLuongMua = soLuong
                });
            }
        }

        public void RemoveFromCart(int maSanPham)
        {
            _currentCart.RemoveAll(item => item.MaSanPham == maSanPham);
        }

        public void ClearCart()
        {
            _currentCart.Clear();
        }
    }
}
