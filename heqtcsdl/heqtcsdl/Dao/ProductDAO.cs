using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heqtcsdl.Dao
{
    public class ProductDAO
    {
        // 1. Lấy toàn bộ danh sách sản phẩm (từ View an toàn)
        public DataTable GetAllProducts()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaSanPham, TenSanPham, Gia, TenLoai FROM dbo.vw_DanhSachSanPham";

            try
            {
                using (SqlConnection conn = new SqlConnection(DbConnection.GetUserConnectionString()))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tải danh sách sản phẩm: " + ex.Message);
            }
            return dt;
        }

        // 2. Tìm kiếm sản phẩm (sử dụng Stored Procedure)
        public DataTable SearchProducts(string searchTerm)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DbConnection.GetUserConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_SearchProducts", conn))
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
                throw new Exception("Lỗi tìm kiếm sản phẩm: " + ex.Message);
            }
            return dt;
        }
    }
}