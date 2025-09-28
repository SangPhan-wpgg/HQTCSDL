using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heqtcsdl.Model
{
    // Class này lưu trữ thông tin của một sản phẩm trong Giỏ hàng
    public class CartItem
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public decimal Gia { get; set; }
        public int SoLuongMua { get; set; }

        // Thuộc tính tính toán
        public decimal ThanhTien
        {
            get { return Gia * SoLuongMua; }
        }
    }
}