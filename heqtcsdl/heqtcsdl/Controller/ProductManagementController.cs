using heqtcsdl.Dao;
using heqtcsdl.Dao.heqtcsdl.Dao;
using heqtcsdl.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heqtcsdl.Controller
{
    public class ProductManagementController
    {
        private readonly ProductManagementDAO _productDAO = new ProductManagementDAO();

        public DataTable LoadAllProducts()
        {
            return _productDAO.GetAllProducts();
        }

        public string HandleAddProduct(Product product)
        {
            return _productDAO.AddProduct(product);
        }

        // THÊM: Xử lý Cập nhật sản phẩm
        public string HandleUpdateProduct(Product product)
        {
            return _productDAO.UpdateProduct(product);
        }

        // THÊM: Xử lý Xóa sản phẩm
        public string HandleDeleteProduct(int maSanPham)
        {
            return _productDAO.DeleteProduct(maSanPham);
        }
    }
}