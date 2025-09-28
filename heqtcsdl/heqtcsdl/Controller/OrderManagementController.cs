using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using heqtcsdl.Dao;
using System.Data;

namespace heqtcsdl.Controller
{
    public class OrderManagementController
    {
        private readonly OrderManagementDAO _orderDAO = new OrderManagementDAO();

        public DataTable LoadAllOrders()
        {
            return _orderDAO.GetAllOrders();
        }

        public string HandleUpdateStatus(int orderId, string newStatus)
        {
            return _orderDAO.UpdateOrderStatus(orderId, newStatus);
        }
    }
}