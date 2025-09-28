using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// File: DbConnection.cs (Trong thư mục Dao)
using heqtcsdl.Model; // Cần sử dụng UserSession để lấy thông tin

namespace heqtcsdl.Dao
{
    public static class DbConnection
    {
        // Chuỗi kết nối tĩnh (Dùng cho usp_DangNhap ban đầu - thường là tài khoản có quyền EXECUTE)
        private const string StaticConnectionString = "Data Source=.;Initial Catalog=ProjectQuanLyCuaHangDienThoai;User ID=SA;Password=YourPasswordHere;TrustServerCertificate=True;";

        // Chuỗi kết nối động: Dùng cho các truy vấn CHỨC NĂNG sau khi đăng nhập
        public static string GetUserConnectionString()
        {
            // Lấy thông tin từ Session đã lưu sau khi đăng nhập thành công
            string username = UserSession.Username;
            string password = UserSession.Password;

            // LƯU Ý: Chỉ nên dùng dấu . (dot) nếu bạn đang chạy trên máy cục bộ với Instance mặc định
            return $"Data Source=.;Initial Catalog=ProjectQuanLyCuaHangDienThoai;User ID={username};Password={password};TrustServerCertificate=True;";
        }

        // Bạn có thể giữ lại hàm này nếu muốn, nhưng hàm GetUserConnectionString() sẽ ưu việt hơn
        /*
        public static string GetDynamicConnectionString(string username, string password)
        {
             return $"Data Source=.;Initial Catalog=ProjectQuanLyCuaHangDienThoai;User ID={username};Password={password};TrustServerCertificate=True;";
        }
        */

        public static string GetStaticConnectionString()
        {
            return StaticConnectionString;
        }
    }
}