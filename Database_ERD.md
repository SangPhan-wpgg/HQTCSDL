```mermaid
erDiagram
    Roles {
        int RoleID PK
        nvarchar RoleName
    }

    Users {
        int UserID PK
        nvarchar Username
        nvarchar PasswordHash
        nvarchar FullName
        nvarchar Email
        varchar SoDienThoai
        nvarchar DiaChi
        bit IsActive
        int RoleID FK
    }

    DanhMucSanPham {
        int DanhMucID PK
        nvarchar TenDanhMuc
    }

    SanPham {
        int SanPhamID PK
        nvarchar TenSanPham
        nvarchar MoTa
        decimal GiaBan
        int SoLuongTon
        int DanhMucID FK
    }

    DonHang {
        int DonHangID PK
        datetime NgayTao
        decimal TongTien
        nvarchar TrangThai
        int KhachHangID FK
        int NhanVienID FK
    }

    ChiTietDonHang {
        int ChiTietID PK
        int SoLuong
        decimal DonGia
        int DonHangID FK
        int SanPhamID FK
    }

    Roles ||--o{ Users : "có"
    Users ||--o{ DonHang : "đặt hàng (khách)"
    Users ||--o{ DonHang : "xử lý (nhân viên)"
    DanhMucSanPham ||--o{ SanPham : "chứa"
    DonHang ||--|{ ChiTietDonHang : "bao gồm"
    SanPham ||--o{ ChiTietDonHang : "có trong"
```
