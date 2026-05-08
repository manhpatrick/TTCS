// auth-guard.js
(function () {
    const path = window.location.pathname.toLowerCase();
    const adminToken = localStorage.getItem('adminToken');
    const userToken = localStorage.getItem('userToken');

    // --- CẤU HÌNH PHÂN QUYỀN ---

    // 1. Danh sách các trang chỉ dành cho ADMIN
    const adminOnlyPages = [
        '/admin_dashboard.html',
        '/admin_room.html',
        '/admin_booking.html',
        '/admin_service.html'
    ];

    // 2. Danh sách các trang chỉ dành cho KHÁCH HÀNG (đã đăng nhập)
    const memberOnlyPages = [
        '/user_profile.html',
        '/booking.html'
    ];

    // 3. Danh sách các trang xác thực (Login/Register)
    const authPages = ['/user_auth.html', '/admin_auth.html'];

    // --- LOGIC KIỂM TRA ---

    // Kiểm tra nếu đang truy cập trang Admin
    const isAdminPage = adminOnlyPages.some(page => path.endsWith(page));
    if (isAdminPage && !adminToken) {
        console.warn("Truy cập trái phép trang Admin. Đang chuyển hướng...");
        window.location.replace('/admin_auth.html');
        return;
    }

    // Kiểm tra nếu đang truy cập trang cá nhân của Khách hàng
    const isMemberPage = memberOnlyPages.some(page => path.endsWith(page));
    if (isMemberPage && !userToken) {
        alert("Vui lòng đăng nhập để tiếp tục.");
        window.location.replace('/user_auth.html');
        return;
    }

    // Nếu đã đăng nhập Admin rồi mà cố vào trang login Admin -> Đẩy vào Dashboard
    if (path.endsWith('/admin_auth.html') && adminToken) {
        window.location.replace('/admin_dashboard.html');
        return;
    }

    // Nếu đã đăng nhập Khách rồi mà cố vào trang login Khách -> Đẩy về Trang chủ
    if (path.endsWith('/user_auth.html') && userToken) {
        window.location.replace('/');
        return;
    }
})();