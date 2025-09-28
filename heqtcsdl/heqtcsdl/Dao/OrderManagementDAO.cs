using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heqtcsdl.Dao
{
    public class OrderManagementDAO
    {
        private string GetConnString() => DbConnection.GetUserConnectionString();

        // 1. Lấy toàn bộ đơn hàng (từ View cho Admin/NV)
        public DataTable GetAllOrders()
        {
            DataTable dt = new DataTable();
            // Cần sử dụng một VIEW toàn bộ đơn hàng (vd: vw_AllOrderList)
            string sql = "SELECT MaDonHang, TenKhachHang, NgayDat, TongTien, TrangThai FROM dbo.vw_AllOrderList ORDER BY NgayDat DESC";

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnString()))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tải danh sách đơn hàng: " + ex.Message);
            }
            return dt;
        }

        // 2. Cập nhật trạng thái đơn hàng (sử dụng SP)
        // Cần SP này trong CSDL: dbo.usp_UpdateOrderStatus
        public string UpdateOrderStatus(int orderId, string newStatus)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_UpdateOrderStatus", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaDonHang", orderId);
                        cmd.Parameters.AddWithValue("@TrangThaiMoi", newStatus);

                        SqlParameter messageParam = cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 255);
                        messageParam.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        return messageParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Lỗi hệ thống khi cập nhật: {ex.Message}";
            }
        }
    }
}