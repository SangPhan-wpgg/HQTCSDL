using heqtcsdl.Model;
using heqtcsdl.Model.heqtcsdl.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heqtcsdl.Dao
{
    public class CustomerDAO
    {
        // 1. Phương thức lấy Thông tin Khách hàng
        public Customer GetCustomerInfo()
        {
            Customer customer = null;
            // Dùng View an toàn đã tạo trước đó
            string sql = "SELECT HoTen, SoDienThoai, TongDiem, HangThanhVien FROM dbo.vw_ThongTinThanhVienCuaToi";

            try
            {
                // SỬA LỖI: Dùng GetUserConnectionString() để đảm bảo RLS hoạt động!
                using (SqlConnection conn = new SqlConnection(DbConnection.GetUserConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Ánh xạ dữ liệu từ CSDL vào Class Customer
                            customer = new Customer
                            {
                                // Giả sử thứ tự cột: HoTen (0), SoDienThoai (1), TongDiem (2), HangThanhVien (3)
                                FullName = reader.GetString(0),
                                PhoneNumber = reader.GetString(1),
                                TotalPoints = reader.GetDecimal(2),
                                MembershipRank = reader.GetString(3)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: mất kết nối CSDL, lỗi phân quyền)
                Console.WriteLine($"Lỗi truy vấn thông tin khách hàng: {ex.Message}");
                throw; // Ném lỗi để Controller và View xử lý hiển thị thông báo
            }
            return customer;
        }

        // 2. Phương thức lấy Lịch sử Đơn hàng (Cần cho CustomerDashboard)
        public DataTable GetOrderHistory()
        {
            DataTable dt = new DataTable();
            // Lấy dữ liệu từ View đã được bảo mật bằng RLS
            string sql = "SELECT MaDonHang, TongTien, NgayDatHang FROM dbo.vw_LichSuDonHangCuaToi ORDER BY NgayDatHang DESC";

            try
            {
                // Dùng GetUserConnectionString() để kết nối bằng login của người dùng
                using (SqlConnection conn = new SqlConnection(DbConnection.GetUserConnectionString()))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi truy vấn lịch sử đơn hàng: {ex.Message}");
                throw;
            }
            return dt;
        }
    }
}