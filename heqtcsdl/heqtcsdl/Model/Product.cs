using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heqtcsdl.Model
{
    public class Product
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public decimal Gia { get; set; }
        // ChiTiet đã bị loại bỏ khỏi View, nhưng có thể giữ ở đây nếu cần cho chi tiết riêng
        public string TenLoai { get; set; }
    }
}