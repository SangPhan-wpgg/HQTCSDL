using heqtcsdl.Dao;
using heqtcsdl.Model;
using heqtcsdl.Model.heqtcsdl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data;

//namespace heqtcsdl.Controller
//{
//    public class CustomerController
//    {
//        private readonly CustomerDAO _customerDAO = new CustomerDAO();

//        // Phương thức đã có
//        public Customer LoadCustomerDashboardData()
//        {
//            return _customerDAO.GetCustomerInfo();
//        }

//        // THÊM PHƯƠNG THỨC NÀY VÀO ĐỂ GIẢI QUYẾT LỖI
//        public DataTable LoadOrderHistory()
//        {
//            // Controller gọi DAO để lấy dữ liệu lịch sử đơn hàng
//            return _customerDAO.GetOrderHistory();
//        }
//    }
//}
using System.Data;

public class CustomerController
{
    private readonly CustomerDAO _customerDAO = new CustomerDAO();

    public Customer LoadCustomerDashboardData()
    {
        return _customerDAO.GetCustomerInfo();
    }

    // Thêm phương thức này để tránh lỗi khi gọi từ LichSuMuaHang
    public DataTable LoadOrderHistory()
    {
        return _customerDAO.GetOrderHistory();
    }
}