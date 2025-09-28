using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; // Ví dụ nếu muốn lưu mật khẩu đã hash

namespace heqtcsdl.Model
{
    /// <summary>
    /// Class tĩnh lưu trữ thông tin phiên làm việc của người dùng hiện tại
    /// </summary>
    public static class UserSession
    {
        // Thông tin xác thực dùng để TẠO KẾT NỐI SQL có phân quyền
        public static string Username { get; private set; }
        public static string Password { get; private set; }

        // Vai trò được xác nhận từ CSDL (sẽ là Admin, NhanVien, hoặc KhachHang)
        public static string UserRole { get; private set; }
        public static int UserId { get; private set; } // Mã người dùng/khách hàng

        /// <summary>
        /// Lưu thông tin phiên làm việc sau khi đăng nhập thành công
        /// </summary>
        public static void SetSession(string username, string password, string role, int userId)
        {
            Username = username;
            Password = password;
            UserRole = role;
            UserId = userId;
        }

        /// <summary>
        /// Xóa phiên làm việc khi đăng xuất
        /// </summary>
        public static void ClearSession()
        {
            Username = null;
            Password = null;
            UserRole = null;
            UserId = 0;
        }

        /// <summary>
        /// Kiểm tra xem người dùng đã đăng nhập chưa
        /// </summary>
        public static bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(Username) && UserId > 0;
        }
    }
}