using heqtcsdl.Model;
using heqtcsdl.Model.heqtcsdl.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace heqtcsdl.Dao
{
    public class CustomerManagementDAO
    {
        // Sử dụng kết nối bảo mật động của Quản lý
        private string GetConnString() => DbConnection.GetUserConnectionString();

        // Lấy danh sách tất cả Khách hàng
        // Quản lý cần xem được cột TrangThaiActive để thực hiện khóa/mở khóa
        public DataTable GetAllCustomers()
        {
            DataTable dt = new DataTable();
            // LƯU Ý: Cần VIEW vw_AllCustomersList trong CSDL, chỉ dành cho Quản lý
            string sql = "SELECT MaThanhVien, TenThanhVien, Email, SDT, DiaChi, DiemTichLuy, TrangThaiActive FROM dbo.vw_AllCustomersList";

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
                throw new Exception("Lỗi tải danh sách khách hàng: " + ex.Message);
            }
            return dt;
        }

        // CẬP NHẬT THÔNG TIN CƠ BẢN CỦA KHÁCH HÀNG
        // Cần SP usp_UpdateCustomerInfo trong CSDL
        public string UpdateCustomerInfo(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(GetConnString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_UpdateCustomerInfo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaThanhVien", customer.MaKhachHang);
                    cmd.Parameters.AddWithValue("@TenMoi", customer.HoTen);
                    cmd.Parameters.AddWithValue("@EmailMoi", customer.Email);
                    cmd.Parameters.AddWithValue("@SDTMoi", customer.SoDienThoai);
                    cmd.Parameters.AddWithValue("@DiaChiMoi", customer.DiaChi);

                    SqlParameter resultParam = cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 255);
                    resultParam.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    return resultParam.Value.ToString();
                }
            }
        }
        public string AddCustomer(Customer customer, string username, string hashedPassword)
        {
            using (SqlConnection conn = new SqlConnection(GetConnString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_AddCustomerAccount", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenThanhVien", customer.HoTen);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@SDT", customer.SoDienThoai);
                    cmd.Parameters.AddWithValue("@DiaChi", customer.DiaChi);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@HashedPassword", hashedPassword);

                    SqlParameter resultParam = cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 255);
                    resultParam.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    return resultParam.Value.ToString();
                }
            }
        }

        // PHƯƠNG THỨC XÓA TÀI KHOẢN KHÁCH HÀNG
        public string DeleteCustomer(int maThanhVien)
        {
            using (SqlConnection conn = new SqlConnection(GetConnString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.usp_DeleteCustomerAccount", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaThanhVien", maThanhVien);

                    SqlParameter resultParam = cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 255);
                    resultParam.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    return resultParam.Value.ToString();
                }
            }
        }
        public DataTable SearchCustomers(string searchTerm)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnString()))
                {
                    conn.Open();
                    // LƯU Ý: Cần SP usp_SearchCustomers trong CSDL
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_SearchCustomers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tìm kiếm khách hàng: " + ex.Message);
            }
            return dt;
        }

    }
}