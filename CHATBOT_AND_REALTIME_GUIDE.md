# Hướng dẫn tích hợp Chatbot AI và Real-time Notifications

## 1. AI Chatbot - Tìm phòng theo yêu cầu

### API Endpoints

#### 1.1 Gửi tin nhắn (Tự động phân tích)
```
POST /api/chatbot/message
Content-Type: application/json

{
  "message": "Tôi muốn tìm phòng giá từ 500k đến 1 triệu",
  "accountId": 1
}
```

**Ví dụ các tin nhắn mà Chatbot hiểu được:**
- "Tôi muốn tìm phòng giá từ 500k đến 1 triệu"
- "Có phòng Deluxe nào không?"
- "Tìm phòng trong khoảng 2 triệu đến 5 triệu"
- "Phòng Standard giá bao nhiêu?"
- "Tôi cần phòng Suite"

**Response:**
```json
{
  "message": "Tôi tìm thấy 5 phòng trong khoảng giá 500000đ - 1000000đ:",
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
  "timestamp": "2026-05-08T10:30:00Z"
}
```

#### 1.2 Tìm kiếm theo khoảng giá
```
GET /api/chatbot/search/by-price?minPrice=500000&maxPrice=1000000
```

**Response:**
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
  "timestamp": "2026-05-08T10:30:00Z"
}
```

#### 1.3 Tìm kiếm theo loại phòng
```
GET /api/chatbot/search/by-category?category=Deluxe
```

**Các loại phòng hợp lệ:**
- Standard
- Deluxe
- Suite
- Presidential

**Response:**
```json
{
  "message": "Tôi tìm thấy 4 phòng Deluxe:",
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
  "timestamp": "2026-05-08T10:30:00Z"
}
```

### Frontend Implementation (React)

#### Setup SignalR connection cho Notifications
```typescript
import * as signalR from "@microsoft/signalr";

// Tạo connection
const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5135/hubs/notification", {
    accessTokenFactory: () => localStorage.getItem("token") || ""
  })
  .withAutomaticReconnect()
  .build();

// Bắt đầu connection
connection.start();

// Lắng nghe thông báo mới
connection.on("ReceiveNotification", (notification) => {
  console.log("Thông báo mới:", notification);
  // Hiển thị badge, sound, toast, etc.
});

// Lắng nghe cập nhật badge
connection.on("UpdateNotificationBadge", (data) => {
  console.log("Số thông báo chưa đọc:", data.unreadCount);
  // Cập nhật UI
});
```

### Frontend - Chatbot Component Example

```typescript
import { useState } from "react";
import axios from "axios";

interface ChatMessage {
  id: string;
  text: string;
  sender: "user" | "bot";
  rooms?: any[];
  timestamp: Date;
}

export const ChatbotWidget = () => {
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [input, setInput] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSendMessage = async () => {
    if (!input.trim()) return;

    // Thêm tin nhắn của user
    const userMessage: ChatMessage = {
      id: Date.now().toString(),
      text: input,
      sender: "user",
      timestamp: new Date()
    };

    setMessages([...messages, userMessage]);
    setInput("");
    setLoading(true);

    try {
      const response = await axios.post(
        "http://localhost:5135/api/chatbot/message",
        { message: input },
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`
          }
        }
      );

      const botMessage: ChatMessage = {
        id: Date.now().toString(),
        text: response.data.message,
        sender: "bot",
        rooms: response.data.rooms,
        timestamp: new Date()
      };

      setMessages(prev => [...prev, botMessage]);
    } catch (error) {
      console.error("Lỗi gửi tin nhắn:", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ width: "400px", border: "1px solid #ccc", borderRadius: "8px", overflow: "hidden" }}>
      <div style={{ height: "400px", overflow: "auto", padding: "16px", backgroundColor: "#f5f5f5" }}>
        {messages.map(msg => (
          <div key={msg.id} style={{ marginBottom: "12px", textAlign: msg.sender === "user" ? "right" : "left" }}>
            <div style={{
              display: "inline-block",
              maxWidth: "80%",
              padding: "8px 12px",
              borderRadius: "8px",
              backgroundColor: msg.sender === "user" ? "#007bff" : "#e9ecef",
              color: msg.sender === "user" ? "white" : "black"
            }}>
              {msg.text}
            </div>
            {msg.rooms && (
              <div style={{ marginTop: "8px" }}>
                {msg.rooms.map(room => (
                  <div key={room.id} style={{
                    padding: "8px",
                    marginTop: "4px",
                    backgroundColor: "white",
                    borderLeft: "4px solid #007bff",
                    fontSize: "12px"
                  }}>
                    <strong>{room.name}</strong> - {room.price.toLocaleString()}đ ({room.category})
                  </div>
                ))}
              </div>
            )}
          </div>
        ))}
        {loading && <div>Đang xử lý...</div>}
      </div>

      <div style={{ padding: "12px", borderTop: "1px solid #ccc", display: "flex", gap: "8px" }}>
        <input
          type="text"
          value={input}
          onChange={(e) => setInput(e.target.value)}
          onKeyPress={(e) => e.key === "Enter" && handleSendMessage()}
          placeholder="Nhập tin nhắn..."
          disabled={loading}
          style={{
            flex: 1,
            padding: "8px",
            border: "1px solid #ddd",
            borderRadius: "4px"
          }}
        />
        <button
          onClick={handleSendMessage}
          disabled={loading}
          style={{
            padding: "8px 16px",
            backgroundColor: "#007bff",
            color: "white",
            border: "none",
            borderRadius: "4px",
            cursor: "pointer"
          }}
        >
          Gửi
        </button>
      </div>
    </div>
  );
};
```

---

## 2. Real-time Notifications

### Backend API

#### 2.1 Gửi thông báo (Admin)
```
POST /api/notification
Content-Type: application/json

{
  "content": "Khách sạn sẽ bảo trì vào 10/05/2026",
  "type": "system"
}
```

**Response:**
```json
{
  "message": "Thông báo đã được gửi"
}
```

#### 2.2 Gửi thông báo cho user cụ thể
```
POST /api/notification/send-to-user/5
Content-Type: application/json

{
  "content": "Booking của bạn đã được xác nhận",
  "type": "booking"
}
```

### Frontend - Real-time Notification Handler

```typescript
import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

export const useNotifications = () => {
  const [notifications, setNotifications] = useState<any[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

  useEffect(() => {
    // Tạo SignalR connection
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5135/hubs/notification", {
        accessTokenFactory: () => localStorage.getItem("token") || ""
      })
      .withAutomaticReconnect()
      .build();

    // Bắt đầu connection
    newConnection.start()
      .then(() => console.log("Connected to Notification Hub"))
      .catch(err => console.log("Connection failed: ", err));

    // Lắng nghe thông báo mới
    newConnection.on("ReceiveNotification", (notification) => {
      setNotifications(prev => [notification, ...prev]);
      setUnreadCount(prev => prev + 1);
      
      // Hiển thị toast notification (tùy chọn)
      showToast(notification.title, notification.message);
    });

    // Lắng nghe cập nhật badge
    newConnection.on("UpdateNotificationBadge", (data) => {
      setUnreadCount(data.unreadCount);
    });

    // Lắng nghe booking mới (admin)
    newConnection.on("NewBooking", (booking) => {
      showToast("Booking mới", booking.message);
    });

    setConnection(newConnection);

    return () => {
      if (newConnection) {
        newConnection.stop();
      }
    };
  }, []);

  return { notifications, unreadCount, connection };
};

// Hook helper để hiển thị toast
const showToast = (title: string, message: string) => {
  // Dùng thư viện toast của bạn (react-toastify, etc.)
  console.log(`${title}: ${message}`);
};

// Component NotificationBell
export const NotificationBell = () => {
  const { unreadCount } = useNotifications();

  return (
    <div style={{ position: "relative", display: "inline-block" }}>
      <button style={{
        fontSize: "20px",
        cursor: "pointer",
        background: "none",
        border: "none"
      }}>
        🔔
      </button>
      {unreadCount > 0 && (
        <span style={{
          position: "absolute",
          top: "-5px",
          right: "-5px",
          backgroundColor: "#dc3545",
          color: "white",
          borderRadius: "50%",
          width: "20px",
          height: "20px",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          fontSize: "12px",
          fontWeight: "bold"
        }}>
          {unreadCount}
        </span>
      )}
    </div>
  );
};
```

### Component ví dụ hoàn chỉnh

```typescript
import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

export const NotificationPanel = () => {
  const [notifications, setNotifications] = useState<any[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const [isOpen, setIsOpen] = useState(false);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5135/hubs/notification", {
        accessTokenFactory: () => localStorage.getItem("token") || ""
      })
      .withAutomaticReconnect()
      .build();

    connection.start();

    connection.on("ReceiveNotification", (notification) => {
      setNotifications(prev => [{
        id: Date.now(),
        ...notification,
        isRead: false
      }, ...prev]);
      setUnreadCount(prev => prev + 1);
    });

    connection.on("UpdateNotificationBadge", (data) => {
      setUnreadCount(data.unreadCount);
    });

    return () => {
      connection.stop();
    };
  }, []);

  return (
    <div>
      {/* Notification Bell */}
      <button
        onClick={() => setIsOpen(!isOpen)}
        style={{
          position: "relative",
          fontSize: "24px",
          background: "none",
          border: "none",
          cursor: "pointer"
        }}
      >
        🔔
        {unreadCount > 0 && (
          <span style={{
            position: "absolute",
            top: "-5px",
            right: "-5px",
            backgroundColor: "#dc3545",
            color: "white",
            borderRadius: "50%",
            padding: "2px 6px",
            fontSize: "12px",
            fontWeight: "bold"
          }}>
            {unreadCount}
          </span>
        )}
      </button>

      {/* Notification Panel */}
      {isOpen && (
        <div style={{
          position: "absolute",
          top: "40px",
          right: "0",
          width: "350px",
          backgroundColor: "white",
          border: "1px solid #ddd",
          borderRadius: "8px",
          boxShadow: "0 2px 10px rgba(0,0,0,0.1)",
          maxHeight: "400px",
          overflow: "auto",
          zIndex: 1000
        }}>
          <div style={{
            padding: "12px",
            borderBottom: "1px solid #eee",
            fontWeight: "bold"
          }}>
            Thông báo ({unreadCount} cái mới)
          </div>

          {notifications.length === 0 ? (
            <div style={{ padding: "20px", textAlign: "center", color: "#999" }}>
              Không có thông báo nào
            </div>
          ) : (
            notifications.map(notif => (
              <div
                key={notif.id}
                style={{
                  padding: "12px",
                  borderBottom: "1px solid #f0f0f0",
                  backgroundColor: notif.isRead ? "#fff" : "#f9f9f9",
                  cursor: "pointer"
                }}
              >
                <div style={{
                  fontWeight: "bold",
                  fontSize: "14px",
                  color: "#333"
                }}>
                  {notif.title}
                </div>
                <div style={{
                  fontSize: "13px",
                  color: "#666",
                  marginTop: "4px"
                }}>
                  {notif.message}
                </div>
                <div style={{
                  fontSize: "11px",
                  color: "#999",
                  marginTop: "4px"
                }}>
                  {new Date(notif.timestamp).toLocaleString("vi-VN")}
                </div>
              </div>
            ))
          )}
        </div>
      )}
    </div>
  );
};
```

---

## 3. Cài đặt Dependencies

### Frontend (React)
```bash
npm install @microsoft/signalr
npm install axios
```

### Backend
Các package đã được thêm:
- `Microsoft.AspNetCore.SignalR` - cho real-time notifications
- `System.Net.Http.Json` - cho HTTP requests

---

## 4. Kiểm tra hoạt động

### Test Chatbot
```bash
# Tìm phòng theo giá
curl -X POST http://localhost:5135/api/chatbot/message \
  -H "Content-Type: application/json" \
  -d '{"message":"Tôi muốn tìm phòng từ 500k đến 1 triệu"}'

# Tìm theo loại phòng
curl -X GET "http://localhost:5135/api/chatbot/search/by-category?category=Deluxe"
```

### Test Real-time Notifications
Dùng Postman hoặc Thunder Client:
```
POST http://localhost:5135/api/notification
Authorization: Bearer [YOUR_TOKEN]
Content-Type: application/json

{
  "content": "Thông báo test",
  "type": "system"
}
```

---

## 5. Ghi chú quan trọng

1. **CORS Settings**: Đã cập nhật CORS để cho phép SignalR connections với `AllowCredentials()`
2. **Token**: SignalR sử dụng JWT token từ `localStorage` để authenticate
3. **Reconnection**: SignalR tự động thử kết nối lại khi mất connection
4. **Message Format**: Chatbot tự động phân tích tin nhắn tự nhiên
5. **Real-time**: Tất cả users nhận được thông báo ngay lập tức mà không cần refresh trang

---

## 6. Mở rộng trong tương lai

1. **Integraton với OpenAI**: Thay thế logic hiện tại bằng GPT API để chatbot thông minh hơn
2. **Lưu lịch chat**: Lưu conversation của user
3. **Push Notifications**: Gửi push notification tới mobile
4. **Email Notifications**: Gửi email cho users quan trọng
5. **Analytics**: Theo dõi và phân tích các yêu cầu của users
