using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heqtcsdl.Model
{
    namespace heqtcsdl.Model
    {
        // Class ánh xạ dữ liệu khách hàng từ CSDL
        public class Customer
        {
            public int CustomerId { get; set; } // MaKhachHang
            public string FullName { get; set; } // HoTen
            public string PhoneNumber { get; set; } // SoDienThoai
            public decimal TotalPoints { get; set; } // TongDiem
            public string MembershipRank { get; set; } // TenHang

            public int MaKhachHang { get; set; } // Sửa từ MaThanhVien
            public string HoTen { get; set; }     // Sửa từ TenThanhVien
            public string Email { get; set; }
            public string SoDienThoai { get; set; } // Sửa từ SDT
            public string DiaChi { get; set; }
            public int TongDiem { get; set; }    // Sửa từ DiemTichLuy
                                                 // Thêm MaNguoiDung để liên kết đến tài khoản đăng nhập (rất quan trọng)
            public int MaNguoiDung { get; set; }
        }
    }
}
