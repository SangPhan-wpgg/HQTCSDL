using heqtcsdl.Dao;
using heqtcsdl.Model.heqtcsdl.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heqtcsdl.Controller
{
    public class CustomerManagementController
    {
        private readonly CustomerManagementDAO _customerDAO = new CustomerManagementDAO();

        public DataTable LoadAllCustomers()
        {
            return _customerDAO.GetAllCustomers();
        }

        public string HandleUpdateCustomer(Customer customer)
        {
            return _customerDAO.UpdateCustomerInfo(customer);
        }

        // THÊM: Xử lý Thêm khách hàng
        public string HandleAddCustomer(Customer customer, string username, string hashedPassword)
        {
            // Lưu ý: Mật khẩu cần được hash ở lớp View TRƯỚC KHI gọi hàm này
            return _customerDAO.AddCustomer(customer, username, hashedPassword);
        }

        // THÊM: Xử lý Xóa khách hàng
        public string HandleDeleteCustomer(int maThanhVien)
        {
            return _customerDAO.DeleteCustomer(maThanhVien);
        }
        public DataTable ExecuteSearch(string searchTerm)
        {
            // Kiểm tra nếu từ khóa rỗng, trả về tất cả khách hàng
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _customerDAO.GetAllCustomers();
            }
            return _customerDAO.SearchCustomers(searchTerm);
        }
    }
}