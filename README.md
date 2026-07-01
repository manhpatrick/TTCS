# HotelManager - Hệ thống quản lý khách sạn trực tuyến

Dự án Hệ thống quản lý khách sạn (Hotel Manager) được xây dựng trong khuôn khổ học phần Thực tập cơ sở, áp dụng kiến trúc Clean Architecture cùng các công nghệ hiện đại như AI và Real-time.

---

## Giới thiệu dự án và Giá trị cốt lõi

Trong bối cảnh chuyển đổi số ngành dịch vụ lưu trú, nhiều cơ sở khách sạn vẫn đang gặp khó khăn với việc quản lý thủ công, dẫn đến sai sót và trải nghiệm khách hàng chưa tốt. Dự án HotelManager được tạo ra nhằm giải quyết vấn đề đó thông qua các giá trị:
* Tập trung hóa dữ liệu: Quản lý đồng bộ từ phòng ốc, dịch vụ, đơn đặt phòng đến thông tin khách hàng trên một nền tảng duy nhất.
* Nâng tầm trải nghiệm: Khách hàng không cần tìm kiếm thủ công nhờ có Trợ lý ảo AI tư vấn, đồng thời nhận thông báo cập nhật trạng thái ngay lập tức mà không cần tải lại trang web.
* Mở rộng dễ dàng: Backend được thiết kế theo chuẩn Clean Architecture 4 lớp, giúp dự án dễ bảo trì và tích hợp thêm các công nghệ mới trong tương lai.

## Chức năng hệ thống

Tùy thuộc vào vai trò, dự án cung cấp bộ công cụ quản lý và tương tác toàn diện:

Dành cho Khách hàng (Customer):
* Chatbot thông minh: Trò chuyện tự nhiên với AI (tích hợp Google Gemini) để tìm phòng theo ngân sách, số lượng người và hạng phòng.
* Đặt phòng và Thanh toán: Thêm dịch vụ đi kèm và thanh toán trực tuyến an toàn qua cổng VNPAY.
* Thông báo Real-time: Nhận thông báo tức thì về trạng thái hóa đơn, xác nhận đặt phòng nhờ công nghệ SignalR.
* Đánh giá và Phản hồi: Gửi phản hồi trực tiếp cho ban quản lý và đánh giá sao sau khi hoàn tất lưu trú.

Dành cho Quản trị viên (Admin):
* Dashboard Thống kê: Theo dõi doanh thu, tỷ lệ đặt phòng qua các biểu đồ trực quan.
* Quản lý Vận hành: Toàn quyền thêm, sửa, xóa thông tin Phòng nghỉ, Dịch vụ và duyệt/hủy các Đơn đặt phòng.
* Tương tác trực tiếp: Trả lời khiếu nại của khách hàng và đẩy thông báo (Push Notification) đến toàn hệ thống.

## Hướng dẫn cài đặt và sử dụng

### 1. Yêu cầu môi trường
* .NET SDK 9.0
* Microsoft SQL Server 2019/2022
* Node.js & npm (Cho phần giao diện)

### 2. Thiết lập Backend (C# .NET)
1. Mở file HotelManagerSolution.sln hoặc mở thư mục gốc bằng IDE.
2. Tại project HotelManager.Presentation, mở file appsettings.json và cấu hình chuỗi kết nối SQL Server cùng API Key của Google Gemini.
3. Chạy lệnh Migration để tự động tạo cơ sở dữ liệu:
   ```bash
   dotnet ef database update --project HotelManager.Infrastructure --startup-project HotelManager.Presentation