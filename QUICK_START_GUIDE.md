# 🚀 Quick Start Guide - Chatbot AI & Real-time Notifications

## Bước 1: Backend Setup (Đã hoàn tất)

### ✅ Các thay đổi đã thực hiện:

#### 1. Thêm Dependencies
- `System.Net.Http.Json` - cho HTTP requests
- SignalR (sẵn có trong ASP.NET Core 9)

#### 2. Tạo các Service và Controller
- **ChatbotService** - Xử lý logic tìm kiếm phòng
- **ChatbotController** - API endpoints cho chatbot
- **NotificationHub** - SignalR hub cho thông báo real-time
- **Cập nhật NotificationController** - Gửi thông báo via SignalR

#### 3. Cấu hình Program.cs
```csharp
// Thêm SignalR
builder.Services.AddSignalR();

// Cập nhật CORS
policy.AllowCredentials();

// Ánh xạ Hub
app.MapHub<NotificationHub>("/hubs/notification");
```

#### 4. Đăng ký Services
```csharp
services.AddScoped<IChatbotService, ChatbotService>();
```

### Build Status: ✅ SUCCESS

```
HotelManager.Domain -> ✓
HotelManager.Application -> ✓
HotelManager.Infrastructure -> ✓
HotelManager.Presentation -> ✓
```

---

## Bước 2: Frontend Setup

### A. Cài đặt Dependencies

```bash
npm install @microsoft/signalr axios
```

### B. Sao chép Components

Copy 4 file từ dự án:

**1. Chatbot Component:**
- Copy: `CHATBOT_COMPONENT_EXAMPLE.tsx` → `src/components/ChatbotWidget.tsx`
- Copy: `CHATBOT_COMPONENT_STYLES.css` → `src/components/ChatbotWidget.css`

**2. Notification Component:**
- Copy: `NOTIFICATION_COMPONENT_EXAMPLE.tsx` → `src/components/NotificationPanel.tsx`
- Copy: `NOTIFICATION_COMPONENT_STYLES.css` → `src/components/NotificationPanel.css`

### C. Import & Sử dụng trong App

```typescript
// App.tsx
import { ChatbotWidget } from './components/ChatbotWidget';
import { NotificationPanel } from './components/NotificationPanel';

export function App() {
  return (
    <div>
      {/* Nội dung app */}
      
      {/* Thêm vào cuối */}
      <NotificationPanel />
      <ChatbotWidget />
    </div>
  );
}
```

---

## Bước 3: Test & Verify

### Test 1: Chatbot API

```bash
# Terminal 1: Chạy backend
cd e:\New folder\HotelManager_TTCS
dotnet run --project HotelManager.Presentation

# Terminal 2: Test API
curl -X POST http://localhost:5135/api/chatbot/message \
  -H "Content-Type: application/json" \
  -d '{"message":"Tôi muốn tìm phòng từ 500k đến 1 triệu"}'
```

**Expected Response:**
```json
{
  "message": "Tôi tìm thấy 3 phòng trong khoảng giá 500000đ - 1000000đ:",
  "rooms": [
    {
      "id": 1,
      "name": "Phòng Deluxe 001",
      "price": 750000,
      "category": "Deluxe",
      "capacity": 2,
      "isAvailable": true
    }
  ],
  "isRoomQuery": true,
  "type": "room_suggestion",
  "timestamp": "2026-05-08T..."
}
```

### Test 2: Real-time Notifications

**Dùng Postman/Thunder Client:**

1. **URL:** `POST http://localhost:5135/api/notification`
2. **Headers:**
   ```
   Authorization: Bearer <YOUR_JWT_TOKEN>
   Content-Type: application/json
   ```
3. **Body:**
   ```json
   {
     "content": "Khách sạn sẽ bảo trì vào 10/05",
     "type": "system"
   }
   ```

4. **Kết quả:** Tất cả clients kết nối sẽ nhận được notification ngay lập tức

---

## Bước 4: Tùy chỉnh (Optional)

### A. Thay đổi mầu sắc

**ChatbotWidget.css:**
```css
/* Thay gradient */
background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);

/* Thành */
background: linear-gradient(135deg, #FF6B6B 0%, #FF5252 100%);
```

### B. Thay đổi API Base URL

**ChatbotWidget.tsx:**
```typescript
const API_BASE = "http://localhost:5135"; // Thay đổi ở đây
```

### C. Thêm Sound Notification

Sẵn có trong `NotificationPanel.tsx` - hàm `playNotificationSound()`

### D. Thêm Browser Notification

```typescript
if ("Notification" in window) {
  Notification.requestPermission();
}
```

---

## Bước 5: Deployment

### Backend Deployment

```bash
# Build for production
dotnet publish -c Release

# Deploy DLL từ bin/Release/net9.0/publish/
```

### Frontend Deployment

```bash
# Build
npm run build

# Deploy từ dist/ folder
```

### Update URLs

Thay đổi API base URL theo domain production:
```typescript
// Development
const API_BASE = "http://localhost:5135";

// Production
const API_BASE = "https://api.hotelmanager.com";
```

---

## Troubleshooting

### ❌ Lỗi: Connection refused
**Giải pháp:** Đảm bảo backend đang chạy
```bash
dotnet run --project HotelManager.Presentation
```

### ❌ Lỗi: CORS error
**Giải pháp:** Frontend URL phải có trong CORS whitelist
```csharp
// Program.cs
policy.WithOrigins("http://localhost:5173") // Thay đổi port nếu cần
```

### ❌ Lỗi: WebSocket connection failed
**Giải pháp:** 
1. Kiểm tra firewall
2. Cập nhật @microsoft/signalr: `npm install @microsoft/signalr@latest`

### ❌ Chatbot không tìm thấy phòng
**Giải pháp:**
1. Kiểm tra database có phòng nào
2. Phòng phải có `RoomStatus = Available`
3. Kiểm tra khoảng giá hợp lệ

---

## 📚 File Structure

```
HotelManager_TTCS/
├── HotelManager.Application/
│   ├── DTO/Chatbot/
│   │   ├── ChatRequest.cs
│   │   └── ChatResponse.cs
│   └── IService/
│       └── IChatbotService.cs
├── HotelManager.Infrastructure/
│   └── Services/
│       └── ChatbotService.cs
├── HotelManager.Presentation/
│   ├── Controllers/
│   │   ├── ChatbotController.cs
│   │   └── Admin/NotificationController.cs
│   ├── Hubs/
│   │   └── NotificationHub.cs
│   └── Program.cs
├── CHATBOT_COMPONENT_EXAMPLE.tsx
├── CHATBOT_COMPONENT_STYLES.css
├── NOTIFICATION_COMPONENT_EXAMPLE.tsx
├── NOTIFICATION_COMPONENT_STYLES.css
├── CHATBOT_AND_REALTIME_GUIDE.md (Chi tiết đầy đủ)
├── IMPLEMENTATION_SUMMARY.md (Tóm tắt)
└── QUICK_START_GUIDE.md (File này)
```

---

## ✨ Features Summary

| Feature | Status | Notes |
|---------|--------|-------|
| 🤖 Chatbot AI | ✅ Ready | Phân tích tự nhiên, tìm phòng |
| 💬 Phân tích tin nhắn | ✅ Ready | Trích xuất giá, loại phòng |
| 📱 Real-time Notifications | ✅ Ready | SignalR WebSockets |
| 🔔 Badge Count | ✅ Ready | Cập nhật tức thời |
| 🎨 UI Components | ✅ Ready | Chatbot widget + Notification panel |
| 📱 Mobile Responsive | ✅ Ready | Responsive design |

---

## 🎯 Next Steps

1. ✅ Copy frontend components
2. ✅ Cài đặt npm dependencies
3. ✅ Test Chatbot API
4. ✅ Test SignalR connection
5. ✅ Tùy chỉnh giao diện
6. ✅ Deployment

---

## 📞 Support

Nếu có vấn đề, kiểm tra:
1. Backend đang chạy? `dotnet run`
2. Frontend dependencies đã cài? `npm install`
3. API URL đúng? Kiểm tra CORS settings
4. JWT token hợp lệ? Cần để authenticate

---

**Bây giờ bạn đã sẵn sàng! 🚀**

Chạy lệnh sau để bắt đầu:
```bash
# Terminal 1
cd e:\New folder\HotelManager_TTCS
dotnet run --project HotelManager.Presentation

# Terminal 2
cd your-react-project
npm start
```

Truy cập: `http://localhost:5173` (hoặc port của bạn)

Bạn sẽ thấy:
- 💬 Chatbot widget ở góc phải dưới
- 🔔 Notification bell ở thanh trên cùng
