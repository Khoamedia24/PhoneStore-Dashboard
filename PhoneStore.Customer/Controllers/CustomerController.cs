using Microsoft.AspNetCore.Mvc;
using PhoneStore.Customer.Models;
using PhoneStore.Customer.ViewModels;
using PhoneStore.Customer.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace PhoneStore.Customer.Controllers
{
    public class CustomerController : Controller
    {
        private readonly PhoneStoreContext _context;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(PhoneStoreContext context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Customer/Register
        public IActionResult Register()
        {
            return View();
        }        // POST: Customer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if email already exists
                    var existingCustomer = await _context.Customers
                        .FirstOrDefaultAsync(c => c.Email == model.Email);

                    if (existingCustomer != null)
                    {
                        ModelState.AddModelError("Email", "Email này đã được sử dụng");
                        return View(model);
                    }                    // Create new customer
                    var customer = new Models.CustomerEntity
                    {
                        Name = $"{model.FirstName} {model.LastName}",
                        Email = model.Email,
                        Phone = model.PhoneNumber,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
                    };

                    _context.Customers.Add(customer);                    await _context.SaveChangesAsync();

                    // Set session
                    HttpContext.Session.SetInt32("CustomerId", customer.Id);
                    HttpContext.Session.SetString("CustomerName", customer.Name ?? customer.Email ?? "");

                    TempData["Success"] = "Đăng ký thành công!";
                    return RedirectToAction("Index", "Home");
                }                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error registering customer");
                    ModelState.AddModelError("", "Có lỗi xảy ra. Vui lòng thử lại sau.");
                }
            }

            return View(model);
        }        // GET: Customer/Login
        public IActionResult Login()
        {
            return View(new Models.ViewModels.LoginViewModel());
        }        // POST: Customer/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Models.ViewModels.LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }            try
            {                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email == model.Email);                if (customer != null && BCrypt.Net.BCrypt.Verify(model.Password, customer.PasswordHash))
                {
                    // Set session
                    HttpContext.Session.SetInt32("CustomerId", customer.Id);
                    HttpContext.Session.SetString("CustomerName", customer.Name ?? customer.Email ?? "");

                    // Handle Remember Me
                    if (model.RememberMe)
                    {
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = Request.IsHttps,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddDays(30)  // Remember for 30 days
                        };

                        Response.Cookies.Append("RememberMe_CustomerId", customer.Id.ToString(), cookieOptions);
                        Response.Cookies.Append("RememberMe_CustomerName", customer.Name ?? customer.Email ?? "", cookieOptions);
                    }

                    TempData["Success"] = "Đăng nhập thành công!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in customer");
                ModelState.AddModelError("", "Có lỗi xảy ra. Vui lòng thử lại sau.");
            }

            return View();
        }        // POST: Customer/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            // Clear remember me cookies
            Response.Cookies.Delete("RememberMe_CustomerId");
            Response.Cookies.Delete("RememberMe_CustomerName");

            TempData["Success"] = "Đăng xuất thành công!";
            return RedirectToAction("Index", "Home");
        }

        // GET: Customer/Profile
        public async Task<IActionResult> Profile()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login");
            }

            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            return View(customer);
        }        // POST: Customer/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(Models.CustomerEntity customer)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCustomer = await _context.Customers.FindAsync(customerId);
                    if (existingCustomer == null)
                    {
                        HttpContext.Session.Clear();
                        return RedirectToAction("Login");
                    }

                    // Update customer info
                    existingCustomer.Name = customer.Name;
                    existingCustomer.Phone = customer.Phone;
                    existingCustomer.BirthYear = customer.BirthYear;

                    await _context.SaveChangesAsync();

                    // Update session
                    HttpContext.Session.SetString("CustomerName", existingCustomer.Name ?? existingCustomer.Email ?? "");

                    TempData["Success"] = "Cập nhật thông tin thành công!";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating customer profile");
                    ModelState.AddModelError("", "Có lỗi xảy ra. Vui lòng thử lại sau.");
                }
            }

            return View(customer);
        }

        // GET: Customer/Orders
        public async Task<IActionResult> Orders()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login");
            }            var orders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // GET: Customer/ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Customer/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("Email", "Vui lòng nhập email");
                return View();
            }

            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email == email);

                if (customer != null)
                {
                    // Generate password reset token
                    var token = Guid.NewGuid().ToString();

                    // Store token in session or database (for demo, using session)
                    HttpContext.Session.SetString($"ResetToken_{email}", token);
                    HttpContext.Session.SetString($"ResetTokenExpiry_{email}", DateTime.UtcNow.AddMinutes(30).ToString());

                    // In real application, send email with reset link
                    // For demo, we'll show success message and provide direct link
                    TempData["Success"] = $"Liên kết đặt lại mật khẩu đã được gửi đến {email}. " +
                                        $"<a href='{Url.Action("ResetPassword", new { token = token, email = email })}' class='text-blue-600 underline'>Nhấn vào đây để đặt lại mật khẩu</a>";
                }
                else
                {
                    // Don't reveal if email exists or not for security
                    TempData["Success"] = $"Nếu email {email} tồn tại trong hệ thống, liên kết đặt lại mật khẩu đã được gửi.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in forgot password");
                TempData["Error"] = "Có lỗi xảy ra. Vui lòng thử lại sau.";
            }

            return View();
        }

        // GET: Customer/ResetPassword
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Liên kết không hợp lệ.";
                return RedirectToAction("Login");
            }

            // Verify token
            var storedToken = HttpContext.Session.GetString($"ResetToken_{email}");
            var expiryStr = HttpContext.Session.GetString($"ResetTokenExpiry_{email}");

            if (storedToken != token || string.IsNullOrEmpty(expiryStr))
            {
                TempData["Error"] = "Liên kết không hợp lệ hoặc đã hết hạn.";
                return RedirectToAction("Login");
            }

            if (DateTime.TryParse(expiryStr, out var expiry) && expiry < DateTime.UtcNow)
            {
                TempData["Error"] = "Liên kết đã hết hạn.";
                return RedirectToAction("Login");
            }

            ViewBag.Token = token;
            ViewBag.Email = email;
            return View();
        }

        // POST: Customer/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string token, string email, string password, string confirmPassword)
        {
            ViewBag.Token = token;
            ViewBag.Email = email;

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                ModelState.AddModelError("Password", "Mật khẩu phải có ít nhất 8 ký tự");
                return View();
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp");
                return View();
            }

            // Verify token again
            var storedToken = HttpContext.Session.GetString($"ResetToken_{email}");
            var expiryStr = HttpContext.Session.GetString($"ResetTokenExpiry_{email}");

            if (storedToken != token || string.IsNullOrEmpty(expiryStr))
            {
                TempData["Error"] = "Liên kết không hợp lệ hoặc đã hết hạn.";
                return View();
            }

            if (DateTime.TryParse(expiryStr, out var expiry) && expiry < DateTime.UtcNow)
            {
                TempData["Error"] = "Liên kết đã hết hạn.";
                return View();
            }

            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email == email);

                if (customer != null)
                {
                    // Update password
                    customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                    await _context.SaveChangesAsync();

                    // Clear reset token
                    HttpContext.Session.Remove($"ResetToken_{email}");
                    HttpContext.Session.Remove($"ResetTokenExpiry_{email}");

                    TempData["Success"] = "Đặt lại mật khẩu thành công! Vui lòng đăng nhập với mật khẩu mới.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy tài khoản.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password");
                TempData["Error"] = "Có lỗi xảy ra. Vui lòng thử lại sau.";
            }

            return View();
        }

        // POST: Customer/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login");
            }

            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                TempData["Error"] = "Vui lòng nhập mật khẩu hiện tại";
                return RedirectToAction("Profile");
            }

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8)
            {
                TempData["Error"] = "Mật khẩu mới phải có ít nhất 8 ký tự";
                return RedirectToAction("Profile");
            }

            if (newPassword != confirmNewPassword)
            {
                TempData["Error"] = "Mật khẩu xác nhận không khớp";
                return RedirectToAction("Profile");
            }

            if (currentPassword == newPassword)
            {
                TempData["Error"] = "Mật khẩu mới phải khác mật khẩu hiện tại";
                return RedirectToAction("Profile");
            }

            try
            {
                var customer = await _context.Customers.FindAsync(customerId);
                if (customer == null)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login");
                }

                // Verify current password
                if (!BCrypt.Net.BCrypt.Verify(currentPassword, customer.PasswordHash))
                {
                    TempData["Error"] = "Mật khẩu hiện tại không đúng";
                    return RedirectToAction("Profile");
                }

                // Update password
                customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đổi mật khẩu thành công!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for customer {CustomerId}", customerId);
                TempData["Error"] = "Có lỗi xảy ra. Vui lòng thử lại sau.";
            }

            return RedirectToAction("Profile");
        }
    }
}
