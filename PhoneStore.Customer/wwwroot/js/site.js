﻿// PhoneStore Customer Site JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Initialize animations
    initializeAnimations();

    // Mobile menu functionality
    initializeMobileMenu();

    // Search functionality
    initializeSearch();

    // Scroll effects
    initializeScrollEffects();

    // Cart functionality
    initializeCart();

    // Update cart count on page load
    updateCartCount();

    // Initialize user menu
    initializeUserMenu();    // Initialize authentication forms
    initializeAuthForms();

    // Initialize password strength indicators
    initializePasswordStrength();

    // Enhance form validation
    enhanceFormValidation();
});

// Initialize page animations using anime.js
function initializeAnimations() {
    // Fade in elements on page load
    anime({
        targets: '.fade-in',
        opacity: [0, 1],
        translateY: [20, 0],
        duration: 800,
        delay: anime.stagger(100),
        easing: 'easeOutQuad'
    });

    // Animate navbar on scroll
    let ticking = false;
    window.addEventListener('scroll', function() {
        if (!ticking) {
            requestAnimationFrame(function() {
                const navbar = document.getElementById('navbar');
                if (window.scrollY > 50) {
                    navbar.classList.add('shadow-xl');
                    navbar.classList.remove('shadow-lg');
                } else {
                    navbar.classList.add('shadow-lg');
                    navbar.classList.remove('shadow-xl');
                }
                ticking = false;
            });
            ticking = true;
        }
    });

    // Product card hover animations
    const productCards = document.querySelectorAll('.product-card');
    productCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            anime({
                targets: this,
                scale: 1.05,
                duration: 300,
                easing: 'easeOutQuad'
            });
        });

        card.addEventListener('mouseleave', function() {
            anime({
                targets: this,
                scale: 1,
                duration: 300,
                easing: 'easeOutQuad'
            });
        });
    });
}

// Mobile menu functionality
function initializeMobileMenu() {
    const mobileMenuBtn = document.querySelector('.mobile-menu-btn');
    const mobileMenu = document.querySelector('.mobile-menu');

    if (mobileMenuBtn && mobileMenu) {
        mobileMenuBtn.addEventListener('click', function() {
            const isHidden = mobileMenu.classList.contains('hidden');

            if (isHidden) {
                mobileMenu.classList.remove('hidden');
                anime({
                    targets: mobileMenu,
                    maxHeight: [0, '300px'],
                    opacity: [0, 1],
                    duration: 300,
                    easing: 'easeOutQuad'
                });
            } else {
                anime({
                    targets: mobileMenu,
                    maxHeight: ['300px', 0],
                    opacity: [1, 0],
                    duration: 300,
                    easing: 'easeOutQuad',
                    complete: function() {
                        mobileMenu.classList.add('hidden');
                    }
                });
            }
        });
    }
}

// Search functionality
function initializeSearch() {
    const searchInput = document.querySelector('input[type="text"][placeholder="Tìm kiếm..."]');

    if (searchInput) {
        let searchTimeout;

        searchInput.addEventListener('input', function() {
            clearTimeout(searchTimeout);
            const query = this.value.trim();

            if (query.length >= 2) {
                searchTimeout = setTimeout(() => {
                    // Perform search (can be enhanced with AJAX)
                    console.log('Searching for:', query);
                }, 300);
            }
        });

        searchInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                const query = this.value.trim();
                if (query) {
                    window.location.href = `/Product?search=${encodeURIComponent(query)}`;
                }
            }
        });
    }
}

// Scroll effects
function initializeScrollEffects() {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate');

                // Animate with anime.js for more complex animations
                if (entry.target.classList.contains('slide-in-left')) {
                    anime({
                        targets: entry.target,
                        translateX: [-50, 0],
                        opacity: [0, 1],
                        duration: 800,
                        easing: 'easeOutQuad'
                    });
                } else if (entry.target.classList.contains('slide-in-right')) {
                    anime({
                        targets: entry.target,
                        translateX: [50, 0],
                        opacity: [0, 1],
                        duration: 800,
                        easing: 'easeOutQuad'
                    });
                }
            }
        });
    }, observerOptions);

    // Observe elements for scroll animations
    document.querySelectorAll('.fade-in, .slide-in-left, .slide-in-right').forEach(el => {
        observer.observe(el);
    });
}

// Cart functionality
function initializeCart() {
    const cartCountElement = document.querySelector('#cart-count');

    // Add to cart function
    window.addToCart = function(productId, productName, price, quantity = 1) {
        // Show loading state
        const button = event.target.closest('button');
        const originalText = button.innerHTML;
        button.innerHTML = '<i class="fas fa-spinner fa-spin mr-2"></i>Đang thêm...';
        button.disabled = true;fetch('/Cart/Add', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                productId: productId,
                quantity: quantity
            })
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Update cart count
                if (cartCountElement) {
                    cartCountElement.textContent = data.cartCount;
                    cartCountElement.style.display = data.cartCount > 0 ? 'flex' : 'none';

                    // Animate cart icon
                    anime({
                        targets: cartCountElement.parentElement,
                        scale: [1, 1.2, 1],
                        duration: 400,
                        easing: 'easeOutQuad'
                    });
                }

                // Show success notification
                showNotification(data.message || `Đã thêm "${productName}" vào giỏ hàng`, 'success');
            } else {
                // Show error notification
                showNotification(data.message || 'Có lỗi xảy ra khi thêm sản phẩm', 'error');
            }
        })
        .catch(error => {
            console.error('Error adding to cart:', error);
            showNotification('Có lỗi xảy ra khi thêm sản phẩm', 'error');
        })
        .finally(() => {
            // Restore button state
            button.innerHTML = originalText;
            button.disabled = false;
        });
    };

    // Add to wishlist function
    window.addToWishlist = function(productId, productName) {
        const button = event.target.closest('button');
        const icon = button.querySelector('i');

        // Toggle heart icon
        if (icon.classList.contains('fas')) {
            icon.classList.remove('fas', 'text-red-500');
            icon.classList.add('far', 'text-gray-600');
            showNotification(`Đã xóa "${productName}" khỏi danh sách yêu thích`, 'info');
        } else {
            icon.classList.remove('far', 'text-gray-600');
            icon.classList.add('fas', 'text-red-500');
            showNotification(`Đã thêm "${productName}" vào danh sách yêu thích`, 'success');
        }

        // Animate icon
        anime({
            targets: icon,
            scale: [1, 1.3, 1],
            duration: 300,
            easing: 'easeOutQuad'
        });
    };    // Quick view function
    window.quickView = function(productId, productName) {
        // Show loading state
        showQuickViewModal(null, true);

        // Fetch product data
        fetch(`/Product/QuickView/${productId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Product not found');
                }
                return response.json();
            })
            .then(data => {
                showQuickViewModal(data, false);
            })
            .catch(error => {
                console.error('Error fetching product data:', error);
                showNotification('Không thể tải thông tin sản phẩm', 'error');
                hideQuickViewModal();
            });
    };
}

// Cart functions
function updateCartCount() {
    fetch('/Cart/GetCount')
        .then(response => response.json())
        .then(data => {
            const cartCount = document.querySelector('.cart-count, #cart-count');
            if (cartCount) {
                cartCount.textContent = data.count;
                cartCount.style.display = data.count > 0 ? 'flex' : 'none';
            }
        })
        .catch(error => console.error('Error updating cart count:', error));
}

// Initialize user menu dropdown
function initializeUserMenu() {
    const userMenuBtn = document.getElementById('user-menu-btn');
    const userDropdown = document.getElementById('user-dropdown');

    if (userMenuBtn && userDropdown) {
        // Toggle dropdown on button click
        userMenuBtn.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();

            const isHidden = userDropdown.classList.contains('hidden');

            if (isHidden) {
                userDropdown.classList.remove('hidden');
                anime({
                    targets: userDropdown,
                    opacity: [0, 1],
                    translateY: [-10, 0],
                    duration: 200,
                    easing: 'easeOutQuad'
                });
            } else {
                anime({
                    targets: userDropdown,
                    opacity: [1, 0],
                    translateY: [0, -10],
                    duration: 200,
                    easing: 'easeOutQuad',
                    complete: function() {
                        userDropdown.classList.add('hidden');
                    }
                });
            }
        });

        // Close dropdown when clicking outside
        document.addEventListener('click', function(e) {
            if (!userMenuBtn.contains(e.target) && !userDropdown.contains(e.target)) {
                if (!userDropdown.classList.contains('hidden')) {
                    anime({
                        targets: userDropdown,
                        opacity: [1, 0],
                        translateY: [0, -10],
                        duration: 200,
                        easing: 'easeOutQuad',
                        complete: function() {
                            userDropdown.classList.add('hidden');
                        }
                    });
                }
            }
        });

        // Close dropdown on escape key
        document.addEventListener('keydown', function(e) {
            if (e.key === 'Escape' && !userDropdown.classList.contains('hidden')) {
                anime({
                    targets: userDropdown,
                    opacity: [1, 0],
                    translateY: [0, -10],
                    duration: 200,
                    easing: 'easeOutQuad',
                    complete: function() {
                        userDropdown.classList.add('hidden');
                    }
                });
            }
        });
    }
}

// Authentication form validation and enhancement
function initializeAuthForms() {
    // Login form validation
    const loginForm = document.getElementById('login-form');
    if (loginForm) {
        loginForm.addEventListener('submit', function(e) {
            const email = document.getElementById('Email').value;
            const password = document.getElementById('Password').value;

            if (!email || !password) {
                e.preventDefault();
                showNotification('Vui lòng điền đầy đủ thông tin!', 'error');
                return;
            }

            if (!isValidEmail(email)) {
                e.preventDefault();
                showNotification('Email không hợp lệ!', 'error');
                return;
            }
        });
    }

    // Register form validation
    const registerForm = document.getElementById('register-form');
    if (registerForm) {
        registerForm.addEventListener('submit', function(e) {
            const fullName = document.getElementById('FullName').value;
            const email = document.getElementById('Email').value;
            const password = document.getElementById('Password').value;
            const confirmPassword = document.getElementById('ConfirmPassword').value;
            const phone = document.getElementById('Phone').value;

            if (!fullName || !email || !password || !confirmPassword || !phone) {
                e.preventDefault();
                showNotification('Vui lòng điền đầy đủ thông tin!', 'error');
                return;
            }

            if (!isValidEmail(email)) {
                e.preventDefault();
                showNotification('Email không hợp lệ!', 'error');
                return;
            }

            if (password !== confirmPassword) {
                e.preventDefault();
                showNotification('Mật khẩu xác nhận không khớp!', 'error');
                return;
            }

            if (password.length < 6) {
                e.preventDefault();
                showNotification('Mật khẩu phải có ít nhất 6 ký tự!', 'error');
                return;
            }

            if (!isValidPhone(phone)) {
                e.preventDefault();
                showNotification('Số điện thoại không hợp lệ!', 'error');
                return;
            }
        });
    }

    // Password toggle functionality
    const togglePasswordButtons = document.querySelectorAll('.toggle-password');
    togglePasswordButtons.forEach(button => {
        button.addEventListener('click', function() {
            const input = this.closest('.relative').querySelector('input');
            const icon = this.querySelector('i');

            if (input.type === 'password') {
                input.type = 'text';
                icon.classList.remove('fa-eye');
                icon.classList.add('fa-eye-slash');
            } else {
                input.type = 'password';
                icon.classList.remove('fa-eye-slash');
                icon.classList.add('fa-eye');
            }
        });
    });

    // Initialize password strength indicators
    initializePasswordStrength();
}

// Password strength checker
function checkPasswordStrength(password) {
    let strength = 0;
    let feedback = [];

    // Length check
    if (password.length >= 8) {
        strength += 1;
    } else {
        feedback.push('Ít nhất 8 ký tự');
    }

    // Uppercase check
    if (/[A-Z]/.test(password)) {
        strength += 1;
    } else {
        feedback.push('Có chữ hoa');
    }

    // Lowercase check
    if (/[a-z]/.test(password)) {
        strength += 1;
    } else {
        feedback.push('Có chữ thường');
    }

    // Number check
    if (/\d/.test(password)) {
        strength += 1;
    } else {
        feedback.push('Có số');
    }

    // Special character check
    if (/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
        strength += 1;
    } else {
        feedback.push('Có ký tự đặc biệt');
    }

    return {
        strength: strength,
        feedback: feedback,
        level: strength <= 2 ? 'weak' : strength <= 3 ? 'medium' : 'strong'
    };
}

// Update password strength indicator
function updatePasswordStrength(passwordInput, strengthIndicator) {
    const password = passwordInput.value;
    const result = checkPasswordStrength(password);

    if (password.length === 0) {
        strengthIndicator.style.display = 'none';
        return;
    }

    strengthIndicator.style.display = 'block';

    const strengthBar = strengthIndicator.querySelector('.strength-bar');
    const strengthText = strengthIndicator.querySelector('.strength-text');
    const strengthFeedback = strengthIndicator.querySelector('.strength-feedback');

    // Update strength bar
    const widthPercent = (result.strength / 5) * 100;
    strengthBar.style.width = `${widthPercent}%`;

    // Update colors and text based on strength level
    strengthBar.className = 'strength-bar transition-all duration-300';
    if (result.level === 'weak') {
        strengthBar.classList.add('bg-red-500');
        strengthText.textContent = 'Yếu';
        strengthText.className = 'strength-text text-red-600 text-sm font-medium';
    } else if (result.level === 'medium') {
        strengthBar.classList.add('bg-yellow-500');
        strengthText.textContent = 'Trung bình';
        strengthText.className = 'strength-text text-yellow-600 text-sm font-medium';
    } else {
        strengthBar.classList.add('bg-green-500');
        strengthText.textContent = 'Mạnh';
        strengthText.className = 'strength-text text-green-600 text-sm font-medium';
    }

    // Update feedback
    if (result.feedback.length > 0) {
        strengthFeedback.textContent = 'Cần: ' + result.feedback.join(', ');
        strengthFeedback.className = 'strength-feedback text-xs text-gray-500 mt-1';
    } else {
        strengthFeedback.textContent = 'Mật khẩu mạnh!';
        strengthFeedback.className = 'strength-feedback text-xs text-green-600 mt-1';
    }
}

// Initialize password strength indicators
function initializePasswordStrength() {
    const passwordInputs = document.querySelectorAll('input[type="password"][data-strength="true"]');

    passwordInputs.forEach(input => {
        const container = input.parentElement;

        // Create strength indicator
        const strengthIndicator = document.createElement('div');
        strengthIndicator.className = 'password-strength mt-2';
        strengthIndicator.style.display = 'none';
        strengthIndicator.innerHTML = `
            <div class="flex items-center justify-between mb-1">
                <span class="strength-text text-sm font-medium">Độ mạnh</span>
            </div>
            <div class="strength-bar-container bg-gray-200 rounded-full h-2 mb-1">
                <div class="strength-bar h-2 rounded-full transition-all duration-300" style="width: 0%"></div>
            </div>
            <div class="strength-feedback text-xs text-gray-500"></div>
        `;

        container.appendChild(strengthIndicator);

        // Add event listener
        input.addEventListener('input', () => {
            updatePasswordStrength(input, strengthIndicator);
        });
    });
}

// Enhanced form validation with better UX
function enhanceFormValidation() {
    const forms = document.querySelectorAll('form[data-validate="true"]');

    forms.forEach(form => {
        const inputs = form.querySelectorAll('input[required]');

        inputs.forEach(input => {
            // Real-time validation
            input.addEventListener('blur', function() {
                validateField(this);
            });

            input.addEventListener('input', function() {
                if (this.classList.contains('error')) {
                    validateField(this);
                }
            });
        });

        // Form submission validation
        form.addEventListener('submit', function(e) {
            let isValid = true;

            inputs.forEach(input => {
                if (!validateField(input)) {
                    isValid = false;
                }
            });

            if (!isValid) {
                e.preventDefault();
                // Focus on first invalid field
                const firstError = form.querySelector('.error');
                if (firstError) {
                    firstError.focus();
                }
            }
        });
    });
}

function validateField(field) {
    const value = field.value.trim();
    const type = field.type;
    let isValid = true;
    let errorMessage = '';

    // Clear previous error state
    field.classList.remove('error');
    const existingError = field.parentElement.querySelector('.field-error');
    if (existingError) {
        existingError.remove();
    }

    // Required validation
    if (field.hasAttribute('required') && !value) {
        isValid = false;
        errorMessage = 'Trường này là bắt buộc';
    }

    // Email validation
    else if (type === 'email' && value && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
        isValid = false;
        errorMessage = 'Email không hợp lệ';
    }

    // Password validation
    else if (type === 'password' && value && value.length < 8) {
        isValid = false;
        errorMessage = 'Mật khẩu phải có ít nhất 8 ký tự';
    }

    // Phone validation
    else if (field.name === 'PhoneNumber' && value && !/^[0-9]{10,11}$/.test(value)) {
        isValid = false;
        errorMessage = 'Số điện thoại không hợp lệ';
    }

    if (!isValid) {
        field.classList.add('error', 'border-red-500', 'focus:border-red-500');

        const errorDiv = document.createElement('div');
        errorDiv.className = 'field-error text-red-500 text-sm mt-1';
        errorDiv.textContent = errorMessage;
        field.parentElement.appendChild(errorDiv);
    } else {
        field.classList.remove('border-red-500', 'focus:border-red-500');
        field.classList.add('border-green-500', 'focus:border-green-500');
    }

    return isValid;
}

// Notification System
function showNotification(message, type = 'info', duration = 3000) {
    // Remove existing notifications
    const existingNotifications = document.querySelectorAll('.toast-notification');
    existingNotifications.forEach(notification => notification.remove());

    // Create notification element
    const notification = document.createElement('div');
    notification.className = `toast-notification fixed top-4 right-4 px-6 py-4 rounded-lg shadow-lg z-50 transform translate-x-full transition-transform duration-300`;

    // Set notification style based on type
    switch (type) {
        case 'success':
            notification.classList.add('bg-green-500', 'text-white');
            break;
        case 'error':
            notification.classList.add('bg-red-500', 'text-white');
            break;
        case 'warning':
            notification.classList.add('bg-yellow-500', 'text-white');
            break;
        case 'info':
        default:
            notification.classList.add('bg-blue-500', 'text-white');
            break;
    }

    // Set notification content
    notification.innerHTML = `
        <div class="flex items-center space-x-3">
            <div class="flex-shrink-0">
                ${getNotificationIcon(type)}
            </div>
            <div class="flex-1">
                <p class="font-medium">${message}</p>
            </div>
            <button onclick="this.parentElement.parentElement.remove()" class="flex-shrink-0 text-white hover:text-gray-200">
                <i class="fas fa-times"></i>
            </button>
        </div>
    `;

    // Add to document
    document.body.appendChild(notification);

    // Animate in
    setTimeout(() => {
        notification.classList.remove('translate-x-full');
    }, 100);

    // Auto remove after duration
    setTimeout(() => {
        notification.classList.add('translate-x-full');
        setTimeout(() => {
            if (notification.parentElement) {
                notification.remove();
            }
        }, 300);
    }, duration);
}

function getNotificationIcon(type) {
    switch (type) {
        case 'success':
            return '<i class="fas fa-check-circle text-lg"></i>';
        case 'error':
            return '<i class="fas fa-exclamation-circle text-lg"></i>';
        case 'warning':
            return '<i class="fas fa-exclamation-triangle text-lg"></i>';
        case 'info':
        default:
            return '<i class="fas fa-info-circle text-lg"></i>';
    }
}

// Helper function to get anti-forgery token
function getAntiForgeryToken() {
    return document.querySelector('meta[name="csrf-token"]')?.getAttribute('content') || '';
}

// Utility functions
function formatPrice(price) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(price);
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Quick View Modal Functions
function showQuickViewModal(productData, isLoading = false) {
    const modal = document.getElementById('quickViewModal');
    const loadingDiv = document.getElementById('quickViewLoading');
    const productDiv = document.getElementById('quickViewProduct');

    if (!modal) {
        console.error('Quick view modal not found');
        return;
    }

    // Reset modal state
    modal.classList.remove('hidden');
    document.body.style.overflow = 'hidden'; // Prevent background scrolling

    // Add accessibility attributes
    modal.setAttribute('aria-hidden', 'false');
    modal.setAttribute('role', 'dialog');
    modal.setAttribute('aria-modal', 'true');

    // Focus management for accessibility
    const firstFocusable = modal.querySelector('button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])');
    if (firstFocusable) {
        setTimeout(() => firstFocusable.focus(), 100);
    }

    // Animate modal in with enhanced effects
    anime({
        targets: modal,
        opacity: [0, 1],
        duration: 300,
        easing: 'easeOutQuad'
    });

    anime({
        targets: modal.querySelector('.bg-white'),
        scale: [0.8, 1],
        opacity: [0, 1],
        duration: 400,
        easing: 'easeOutBack'
    });

    if (isLoading || !productData) {
        // Show loading state with spinner animation
        loadingDiv.classList.remove('hidden');
        productDiv.classList.add('hidden');

        // Animate loading spinner
        const spinner = loadingDiv.querySelector('.fa-spinner');
        if (spinner) {
            spinner.classList.add('quick-view-spinner');
        }
    } else {
        // Hide loading and show product data
        loadingDiv.classList.add('hidden');
        productDiv.classList.remove('hidden');
        populateQuickViewData(productData);
    }
}

function hideQuickViewModal() {
    const modal = document.getElementById('quickViewModal');
    if (!modal) return;

    // Restore accessibility
    modal.setAttribute('aria-hidden', 'true');

    // Animate modal out
    anime({
        targets: modal,
        opacity: [1, 0],
        duration: 300,
        easing: 'easeOutQuad',
        complete: function() {
            modal.classList.add('hidden');
            document.body.style.overflow = ''; // Restore scrolling

            // Reset quantity to 1 when closing
            const quantityInput = document.getElementById('quickViewQuantity');
            if (quantityInput) {
                quantityInput.value = 1;
            }
        }
    });

    anime({
        targets: modal.querySelector('.bg-white'),
        scale: [1, 0.8],
        opacity: [1, 0],
        duration: 300,
        easing: 'easeOutQuad'
    });
}

function populateQuickViewData(data) {
    try {
        // Update product name and category with error handling
        const nameEl = document.getElementById('quickViewProductName');
        const categoryEl = document.getElementById('quickViewCategoryName');

        if (nameEl) nameEl.textContent = data.name || 'Sản phẩm';
        if (categoryEl) categoryEl.textContent = data.categoryName || '';

        // Update description with fallback
        const descriptionEl = document.getElementById('quickViewDescription');
        if (descriptionEl) {
            descriptionEl.textContent = data.description || 'Không có mô tả sản phẩm';
        }

        // Update prices with enhanced formatting
        const currentPriceEl = document.getElementById('quickViewCurrentPrice');
        const originalPriceEl = document.getElementById('quickViewOriginalPrice');
        const discountEl = document.getElementById('quickViewDiscount');

        if (currentPriceEl) {
            if (data.discountPrice && data.discountPrice < data.price) {
                currentPriceEl.textContent = formatPrice(data.discountPrice);
                if (originalPriceEl) {
                    originalPriceEl.textContent = formatPrice(data.price);
                    originalPriceEl.classList.remove('hidden');
                }

                if (discountEl) {
                    const discountPercent = Math.round(((data.price - data.discountPrice) / data.price) * 100);
                    discountEl.textContent = `-${discountPercent}%`;
                    discountEl.classList.remove('hidden');
                }
            } else {
                currentPriceEl.textContent = formatPrice(data.price || 0);
                if (originalPriceEl) originalPriceEl.classList.add('hidden');
                if (discountEl) discountEl.classList.add('hidden');
            }
        }

        // Update stock status with enhanced UI
        updateStockStatus(data.stockQuantity || 0);

        // Update images with error handling
        updateQuickViewImages(data.images || [], data.name || '');

        // Update detail link
        const detailLink = document.getElementById('quickViewDetailsLink');
        if (detailLink && data.id) {
            detailLink.href = `/Product/Detail/${data.id}`;
        }

        // Setup action buttons with enhanced error handling
        setupQuickViewActions(data);

    } catch (error) {
        console.error('Error populating quick view data:', error);
        showNotification('Có lỗi khi hiển thị thông tin sản phẩm', 'error');
    }
}

function updateStockStatus(stockQuantity) {
    const stockEl = document.getElementById('quickViewStock');
    const stockQuantityEl = document.getElementById('quickViewStockQuantity');

    if (!stockEl) return;

    if (stockQuantity > 0) {
        stockEl.innerHTML = '<i class="fas fa-check-circle text-green-500"></i><span class="text-green-600 font-medium">Còn hàng</span>';
        if (stockQuantityEl) {
            stockQuantityEl.textContent = `(${stockQuantity} sản phẩm)`;
        }
    } else {
        stockEl.innerHTML = '<i class="fas fa-times-circle text-red-500"></i><span class="text-red-600 font-medium">Hết hàng</span>';
        if (stockQuantityEl) {
            stockQuantityEl.textContent = '';
        }
    }
}

function updateQuickViewImages(images, productName) {
    const mainImageEl = document.getElementById('quickViewMainImage');
    const thumbnailsEl = document.getElementById('quickViewThumbnails');

    if (!mainImageEl || !thumbnailsEl) return;

    if (images && images.length > 0) {
        // Set main image with error handling
        mainImageEl.src = images[0];
        mainImageEl.alt = productName;
        mainImageEl.onerror = function() {
            this.src = '/images/no-image.svg';
            this.alt = 'Không có hình ảnh';
        };

        // Clear and populate thumbnails
        thumbnailsEl.innerHTML = '';
        images.forEach((image, index) => {
            const thumb = document.createElement('div');
            thumb.className = `quick-view-thumbnail w-16 h-16 bg-gray-100 rounded-lg overflow-hidden cursor-pointer border-2 ${index === 0 ? 'border-primary-500 active' : 'border-transparent'} hover:border-primary-300 transition-all`;

            const img = document.createElement('img');
            img.src = image;
            img.alt = productName;
            img.className = 'w-full h-full object-cover';
            img.onerror = function() {
                this.src = '/images/no-image.svg';
            };

            thumb.appendChild(img);
            thumb.onclick = () => changeQuickViewImage(image, thumb);
            thumbnailsEl.appendChild(thumb);
        });
    } else {
        // Use placeholder image
        mainImageEl.src = '/images/no-image.svg';
        mainImageEl.alt = 'Không có hình ảnh';
        thumbnailsEl.innerHTML = '';
    }
}

function setupQuickViewActions(data) {
    // Setup add to cart button
    const addToCartBtn = document.getElementById('quickViewAddToCart');
    if (addToCartBtn) {
        addToCartBtn.onclick = () => {
            const quantity = parseInt(document.getElementById('quickViewQuantity')?.value) || 1;
            const price = data.discountPrice || data.price || 0;

            if (data.stockQuantity <= 0) {
                showNotification('Sản phẩm hiện tại đã hết hàng', 'warning');
                return;
            }

            if (quantity > data.stockQuantity) {
                showNotification(`Chỉ còn ${data.stockQuantity} sản phẩm trong kho`, 'warning');
                return;
            }

            addToCart(data.id, data.name, price, quantity);
        };
    }

    // Setup add to wishlist button
    const addToWishlistBtn = document.getElementById('quickViewAddToWishlist');
    if (addToWishlistBtn) {
        addToWishlistBtn.onclick = () => addToWishlist(data.id, data.name);
    }

    // Disable/enable add to cart based on stock
    if (addToCartBtn) {
        if (data.stockQuantity <= 0) {
            addToCartBtn.disabled = true;
            addToCartBtn.classList.add('opacity-50', 'cursor-not-allowed');
            addToCartBtn.innerHTML = '<i class="fas fa-times"></i><span>Hết hàng</span>';
        } else {
            addToCartBtn.disabled = false;
            addToCartBtn.classList.remove('opacity-50', 'cursor-not-allowed');
            addToCartBtn.innerHTML = '<i class="fas fa-shopping-cart"></i><span>Thêm vào giỏ hàng</span>';
        }
    }

    // Set max quantity for input
    const quantityInput = document.getElementById('quickViewQuantity');
    if (quantityInput) {
        quantityInput.max = data.stockQuantity || 1;
        quantityInput.value = Math.min(parseInt(quantityInput.value) || 1, data.stockQuantity || 1);
    }
}

function changeQuickViewImage(imageSrc, thumbElement) {
    // Update main image
    const mainImage = document.getElementById('quickViewMainImage');
    if (mainImage) {
        mainImage.src = imageSrc;

        // Add loading animation
        mainImage.style.opacity = '0.5';
        mainImage.onload = function() {
            anime({
                targets: mainImage,
                opacity: [0.5, 1],
                duration: 300,
                easing: 'easeOutQuad'
            });
        };
    }

    // Update thumbnail borders
    const thumbnails = document.querySelectorAll('#quickViewThumbnails .quick-view-thumbnail');
    thumbnails.forEach(thumb => {
        thumb.classList.remove('border-primary-500', 'active');
        thumb.classList.add('border-transparent');
    });

    if (thumbElement) {
        thumbElement.classList.remove('border-transparent');
        thumbElement.classList.add('border-primary-500', 'active');
    }
}

// Enhanced quantity selector functions with validation
function increaseQuantity() {
    const quantityInput = document.getElementById('quickViewQuantity');
    if (quantityInput) {
        const currentValue = parseInt(quantityInput.value) || 1;
        const maxValue = parseInt(quantityInput.max) || 999;

        if (currentValue < maxValue) {
            quantityInput.value = currentValue + 1;

            // Add visual feedback
            anime({
                targets: quantityInput,
                scale: [1, 1.1, 1],
                duration: 200,
                easing: 'easeOutQuad'
            });
        } else {
            showNotification(`Chỉ còn ${maxValue} sản phẩm trong kho`, 'warning');
        }
    }
}

function decreaseQuantity() {
    const quantityInput = document.getElementById('quickViewQuantity');
    if (quantityInput) {
        const currentValue = parseInt(quantityInput.value) || 1;

        if (currentValue > 1) {
            quantityInput.value = currentValue - 1;

            // Add visual feedback
            anime({
                targets: quantityInput,
                scale: [1, 1.1, 1],
                duration: 200,
                easing: 'easeOutQuad'
            });
        }
    }
}

// Close modal when clicking outside
document.addEventListener('click', function(e) {
    const modal = document.getElementById('quickViewModal');
    if (modal && e.target === modal) {
        hideQuickViewModal();
    }
});

// Close modal with Escape key
document.addEventListener('keydown', function(e) {
    if (e.key === 'Escape') {
        const modal = document.getElementById('quickViewModal');
        if (modal && !modal.classList.contains('hidden')) {
            hideQuickViewModal();
        }
    }
});
