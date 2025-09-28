// Bạn cần cài đặt Newtonsoft.Json qua NuGet
// Trong VS: Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution...
// Tìm và cài đặt: Newtonsoft.Json

using heqtcsdl.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heqtcsdl.Dao
{
    public class TransactionDAO
    {
        /// <summary>
        /// Gọi Stored Procedure dbo.usp_TaoDonHang để đặt hàng
        /// </summary>
        /// <param name="cartItems">Danh sách sản phẩm trong giỏ hàng</param>
        /// <returns>Chuỗi thông báo thành công hoặc thất bại</returns>
        public string CreateOrder(List<CartItem> cartItems)
        {
            if (cartItems == null || cartItems.Count == 0)
            {
                return "Giỏ hàng trống.";
            }

            // Chuyển đổi List<CartItem> thành chuỗi JSON
            string cartJson = JsonConvert.SerializeObject(cartItems);

            try
            {
                // Dùng GetUserConnectionString() để đảm bảo SP biết MaKhachHang là ai
                using (SqlConnection conn = new SqlConnection(DbConnection.GetUserConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_TaoDonHang", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CartJson", cartJson);

                        // Tham số OUTPUT
                        SqlParameter messageParam = cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 255);
                        messageParam.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        return messageParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Lỗi kết nối hoặc hệ thống: {ex.Message}";
            }
        }
    }
}
