using heqtcsdl.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;   
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace heqtcsdl.Dao
{
    namespace heqtcsdl.Dao
    {
        public class ProductManagementDAO
        {
            // Sử dụng kết nối bảo mật động của Quản lý/Nhân viên
            private string GetConnString() => DbConnection.GetUserConnectionString();

            // Lấy danh sách sản phẩm (sử dụng View đã có)
            public DataTable GetAllProducts()
            {
                DataTable dt = new DataTable();
                // Quản lý/NV có thể truy cập View này hoặc một View chi tiết hơn nếu cần
                string sql = "SELECT MaSanPham, TenSanPham, Gia, TenLoai FROM dbo.vw_DanhSachSanPham";

                using (SqlConnection conn = new SqlConnection(GetConnString()))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    da.Fill(dt);
                }
                return dt;
            }

            // THÊM SẢN PHẨM (Giả sử có SP usp_AddProduct)
            // Đây là ví dụ, bạn cần tạo SP này trong CSDL
            public string AddProduct(Product product)
            {
                using (SqlConnection conn = new SqlConnection(GetConnString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_AddProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TenSanPham", product.TenSanPham);
                        cmd.Parameters.AddWithValue("@Gia", product.Gia);
                        // Giả sử có thêm MaLoai

                        SqlParameter resultParam = cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 255);
                        resultParam.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        return resultParam.Value.ToString();
                    }
                }
            }
            public string UpdateProduct(Product product)
            {
                using (SqlConnection conn = new SqlConnection(GetConnString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_UpdateProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        // Truyền MaSanPham để biết bản ghi nào cần sửa
                        cmd.Parameters.AddWithValue("@MaSanPham", product.MaSanPham);
                        cmd.Parameters.AddWithValue("@TenSanPham", product.TenSanPham);
                        cmd.Parameters.AddWithValue("@Gia", product.Gia);

                        SqlParameter resultParam = cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 255);
                        resultParam.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        return resultParam.Value.ToString();
                    }
                }
            }

            public string DeleteProduct(int maSanPham)
            {
                using (SqlConnection conn = new SqlConnection(GetConnString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_DeleteProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaSanPham", maSanPham);

                        SqlParameter resultParam = cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 255);
                        resultParam.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        return resultParam.Value.ToString();
                    }
                }
            }
        }
    }
}
