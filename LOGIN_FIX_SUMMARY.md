# Giải Quyết Vấn Đề Đăng Nhập Admin - PhoneStore

## 🎯 Vấn đề gốc
Không thể đăng nhập với tài khoản admin (username: "admin", password: "123")

## 🔧 Các lỗi đã được sửa

### 1. **Lỗi mã hóa mật khẩu**
**Vấn đề:** Phương thức `VerifyPassword` sử dụng `Convert.ToBase64String()` nhưng database lưu hash dưới dạng hex string.

**Giải pháp:**
```csharp
// TRƯỚC (sai):
var hash = Convert.ToBase64String(hashedBytes);

// SAU (đúng):
var hash = Convert.ToHexString(hashedBytes).ToLower();
```

### 2. **Thiếu cấu hình Cookie Authentication**
**Vấn đề:** `Program.cs` chỉ có session mà không có cookie authentication configuration.

**Giải pháp:** Thêm vào `Program.cs`:
```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/AdminAccount/Login";
        options.LogoutPath = "/AdminAccount/Logout";
        options.AccessDeniedPath = "/AdminAccount/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });
```

### 3. **Thứ tự middleware sai**
**Vấn đề:** Thứ tự middleware trong pipeline không đúng.

**Giải pháp:** Sắp xếp lại thứ tự:
```csharp
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();           // Sau UseRouting
app.UseAuthentication();    // Trước UseAuthorization
app.UseAuthorization();
```

### 4. **Lỗi hiển thị error message**
**Vấn đề:** View sử dụng `ViewBag.Error` nhưng controller set `TempData["Error"]`.

**Giải pháp:** Sửa trong `Login.cshtml`:
```html
@if (TempData["Error"] != null)
{
    <div class="mb-4 p-4 rounded flex items-center">
        <span class="font-medium">@TempData["Error"]</span>
    </div>
}
```

### 5. **Thiếu Anti-Forgery Token**
**Vấn đề:** Form không có anti-forgery token nhưng controller có `[ValidateAntiForgeryToken]`.

**Giải pháp:** Thêm vào form:
```html
<form asp-action="Login" method="post" class="space-y-5">
    @Html.AntiForgeryToken()
    <!-- form fields -->
</form>
```

## ✅ Thông tin đăng nhập

### Tài khoản admin mặc định:
- **Username:** `admin`
- **Password:** `123`
- **Role:** SuperAdmin
- **Hash trong DB:** `a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3`

## 🧪 Cách test

### 1. Truy cập trang debug:
```
http://localhost:5000/AdminAccount/Debug
```
Kiểm tra:
- Password hash có khớp không
- Admin account có tồn tại trong database không
- Database connection có hoạt động không

### 2. Truy cập trang login:
```
http://localhost:5000/AdminAccount/Login
```
Đăng nhập với:
- Username: `admin`
- Password: `123`

### 3. Sau khi đăng nhập thành công:
- Sẽ redirect đến `/Home/Index`
- Claims authentication hoạt động
- Role-based authorization hoạt động

## 🔐 Hệ thống phân quyền

### Roles được seed:
1. **SuperAdmin** (RoleId: 1) - Toàn quyền hệ thống
2. **Admin** (RoleId: 2) - Quản lý hệ thống  
3. **User** (RoleId: 3) - Người dùng thông thường

### Permissions được seed:
1. **ViewProducts** - Xem danh sách sản phẩm
2. **CreateProduct** - Thêm sản phẩm mới
3. **EditProduct** - Sửa sản phẩm
4. **DeleteProduct** - Xóa sản phẩm
5. **ManageRoles** - Quản lý phân quyền

## 🚀 Trạng thái hiện tại

✅ **Hoàn thành**: Tất cả lỗi compilation đã được sửa
✅ **Hoàn thành**: Database schema clean và có seed data
✅ **Hoàn thành**: Cookie authentication hoạt động
✅ **Hoàn thành**: Claims-based authorization hoạt động
✅ **Hoàn thành**: Đăng nhập admin/123 thành công

**Hệ thống đã sẵn sàng để sử dụng!** 🎉
