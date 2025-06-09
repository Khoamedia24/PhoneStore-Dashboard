# PhoneStore - Ứng dụng Quản lý Cửa hàng Điện thoại

Đây là ứng dụng quản lý cửa hàng điện thoại được phát triển bằng ASP.NET Core MVC.

## Tính năng

- Quản lý sản phẩm (thêm, sửa, xóa, tìm kiếm)
- Quản lý danh mục sản phẩm
- Quản lý màu sắc sản phẩm
- Quản lý đơn hàng với theo dõi trạng thái
- Quản lý khách hàng
- Phân quyền người dùng
- Thống kê doanh thu

## Yêu cầu hệ thống

- .NET 8.0 SDK trở lên
- SQL Server
- Visual Studio 2022 hoặc Visual Studio Code

## Cài đặt

1. Clone repository này về máy của bạn:
```
git clone https://github.com/Khoamedia24/PhoneStore-Dashboard.git
```

2. Di chuyển vào thư mục dự án:
```
cd PhoneStore-Dashboard
```

3. Cấu hình cơ sở dữ liệu:
   - Tạo một bản sao của file `PhoneStore/appsettings.example.json` và đổi tên thành `PhoneStore/appsettings.json`
   - Cập nhật chuỗi kết nối trong file `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "PhoneStoreConnection": "Server=YOUR_SERVER;Database=PhoneStore;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

4. Thiết lập dự án sử dụng script tiện ích:
```
cd PhoneStore
.\project_utils.ps1 setup
```

5. Thiết lập quyền và vai trò (chỉ cần làm nếu không thực hiện bước 4 hoặc cần thiết lập lại):
```
.\project_utils.ps1 seed-perms
```

6. Chạy ứng dụng:
```
.\project_utils.ps1 run
```
Hoặc sử dụng lệnh .NET CLI:
```
dotnet run --urls=http://localhost:5000
```

6. Truy cập ứng dụng:
   - Mở trình duyệt và truy cập: `http://localhost:5000`
   - Đăng nhập với tài khoản admin mặc định:
     - Tên đăng nhập: admin1
     - Mật khẩu: Admin@123

## Cấu trúc dự án

- **Controllers/**: Chứa các controller xử lý các yêu cầu HTTP
- **Models/**: Chứa các lớp định nghĩa cấu trúc dữ liệu của ứng dụng
- **Views/**: Chứa các file giao diện người dùng
- **Services/**: Chứa các dịch vụ xử lý logic nghiệp vụ
- **Migrations/**: Chứa các migration của Entity Framework Core
- **wwwroot/**: Chứa các tài nguyên tĩnh như CSS, JavaScript, hình ảnh, ...

## Hướng dẫn sử dụng

### Đăng nhập hệ thống
- Đường dẫn: `/AdminAccount/Login`
- Tài khoản mặc định: admin1 / Admin@123

### Quản lý sản phẩm
- Thêm, sửa, xóa sản phẩm
- Quản lý hình ảnh sản phẩm theo màu sắc
- Áp dụng giảm giá cho sản phẩm

### Quản lý đơn hàng
- Xem danh sách đơn hàng
- Thay đổi trạng thái đơn hàng (Đang xử lý, Đã xác nhận, Đang giao hàng, Đã giao hàng, Đã hủy)
- Xem chi tiết đơn hàng
- Thêm ghi chú và cập nhật thông tin thanh toán

## Công cụ tiện ích

Dự án này bao gồm các script PowerShell tiện ích để giúp bạn quản lý và bảo trì ứng dụng:

### project_utils.ps1
Script tiện ích chính với các lệnh:
```
.\project_utils.ps1 <command>
```
Các lệnh có sẵn:
- `setup`: Thiết lập dự án (build, áp dụng migrations, cài đặt database)
- `run`: Chạy ứng dụng
- `clean`: Dọn dẹp dự án (xóa bin, obj, v.v.)
- `optimize`: Tối ưu hóa các file hình ảnh lớn
- `migrations`: Liệt kê tất cả migrations
- `update-db`: Cập nhật database lên phiên bản mới nhất
- `help`: Hiển thị trợ giúp

### cleanup.ps1
Dọn dẹp các file không cần thiết trong dự án:
```
.\cleanup.ps1
```

### optimize_images.ps1
Tối ưu hóa các file hình ảnh lớn:
```
.\optimize_images.ps1
```

## Hệ thống phân quyền

Ứng dụng sử dụng hệ thống phân quyền dựa trên vai trò (Role-Based Access Control) với ba vai trò chính:

1. **SuperAdmin**: Có toàn quyền trên hệ thống, bao gồm quản lý vai trò và tài khoản quản trị.
2. **Admin**: Có quyền quản lý hầu hết các tính năng trừ quản lý vai trò và tài khoản quản trị.
3. **User**: Chỉ có quyền xem các thông tin cơ bản.

Khi khởi tạo hệ thống bằng lệnh `project_utils.ps1 setup` hoặc `project_utils.ps1 seed-perms`, hệ thống sẽ tự động tạo tài khoản admin1 với quyền SuperAdmin nếu chưa tồn tại.

```
Username: admin1
Password: Admin@123
```

### Danh sách quyền

Hệ thống bao gồm các quyền được phân theo từng khu vực quản lý:

- Quản lý sản phẩm: ViewProducts, CreateProduct, EditProduct, DeleteProduct
- Quản lý danh mục: ViewCategories, CreateCategory, EditCategory, DeleteCategory
- Quản lý màu sắc: ViewColors, CreateColor, EditColor, DeleteColor
- Quản lý giảm giá: ViewDiscounts, CreateDiscount, EditDiscount, DeleteDiscount
- Quản lý đơn hàng: ViewOrders, CreateOrder, EditOrder, DeleteOrder
- Quản lý khách hàng: ViewCustomers, CreateCustomer, EditCustomer, DeleteCustomer
- Quản lý tài khoản: ViewAdmins, CreateAdmin, EditAdmin, DeleteAdmin
- Quản lý phân quyền: ViewRoles, CreateRole, EditRole, DeleteRole
- Quản lý dashboard: ViewDashboard, GenerateFakeOrders