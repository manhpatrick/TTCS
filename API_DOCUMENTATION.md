# 🏨 Hotel Manager API Documentation

## 🔷 Base URL
```
https://localhost:5135
```

## 🔷 Authentication
All endpoints (except **Auth**) require JWT token in header:
```javascript
Authorization: Bearer <your_jwt_token>
```

---

## 📌 AUTH ENDPOINTS (Public - No Auth Required)

### 1. **POST** `/api/auth/register`
Đăng ký tài khoản mới
```javascript
// Request Body
{
  "username": "john_doe",
  "password": "password123",
  "email": "john@example.com",
  "name": "John Doe",
  "phone": "0987654321",
  "address": "123 Main St",
  "gender": 0, // 0: Male, 1: Female, 2: Other
  "birthDay": "1990-01-15"
}

// Response
{ }
```

### 2. **POST** `/api/auth/login`
Đăng nhập
```javascript
// Request Body
{
  "username": "john_doe",
  "password": "password123"
}

// Response 200
{
  "accountId": 1,
  "username": "john_doe",
  "role": "Customer",
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 3600
}
```

---

## 📌 CUSTOMER ENDPOINTS (Protected - Requires JWT & Role: Customer)

### **User Management**

#### 3. **GET** `/api/customer/user/me`
Lấy thông tin cá nhân của chính user đang đăng nhập

Ghi chú:
- Endpoint này chỉ cho role Customer.
- Hệ thống tự lấy accountId từ JWT claim (NameIdentifier), không cần truyền userId.
```javascript
// Response 200
{
  "name": "John Doe",
  "email": "john@example.com",
  "phone": "0987654321",
  "address": "123 Main St",
  "gender": 1,
  "birthDay": "1990-01-15"
}
```

```javascript
// gender enum
// 1: Male, 2: Female, 3: Other
```

#### 4. **PUT** `/api/customer/user/me`
Cập nhật thông tin cá nhân của chính user đang đăng nhập

Ghi chú:
- Endpoint này chỉ cho role Customer.
- Hệ thống tự lấy accountId từ JWT claim (NameIdentifier), không cần truyền userId.
- Hỗ trợ cập nhật từng phần: chỉ field nào gửi lên (khác null) mới được cập nhật.
```javascript
// Request Body
{
  "name": "John Updated",
  "email": "john.new@example.com",
  "phone": "0987654321",
  "address": "456 Oak Ave",
  "gender": 1,
  "birthDay": "1990-01-15"
}

// Response 200
{ }
```

```javascript
// gender enum
// 1: Male, 2: Female, 3: Other
```

### **Booking Management**

#### 5. **POST** `/api/customer/booking`
Đặt phòng
```javascript
// Request Body
{
  "roomId": 1,
  "numOfPeople": 2,
  "startTime": "2026-04-20T10:00:00",
  "endTime": "2026-04-22T10:00:00",
  "note": "Need late check-in"
}

// Response 200
{ }
```

#### 6. **GET** `/api/customer/booking/me`
Lấy danh sách đặt phòng của tôi
```javascript
// Response 200
[
  {
    "id": 1,
    "roomId": 1,
    "roomName": "Ocean View Suite",
    "numOfPeople": 2,
    "startTime": "2026-04-20T10:00:00",
    "endTime": "2026-04-22T10:00:00",
    "bookingStatus": "Approved",
    "totalPrice": 500000,
    "roomPriceAtBooking": 250000,
    "createdAt": "2026-04-10T15:30:00",
    "approvedAt": "2026-04-10T16:00:00",
    "note": "Need late check-in"
  }
]
```

### **Notification Management**

#### 7. **GET** `/api/customer/notification/me`
Lấy thông báo của tôi
```javascript
// Response 200
[
  {
    "id": 1,
    "content": "Your booking has been approved",
    "createdAt": "2026-04-10T16:00:00",
    "isRead": false,
    "readAt": null
  }
]
```

#### 8. **PUT** `/api/customer/notification/{notificationId}`
Đánh dấu thông báo đã đọc
```javascript
// Response 200
{ }
```

---

## 📌 ADMIN ENDPOINTS (Protected - Requires JWT & Role: Admin)

### **Room Management**

#### 9. **GET** `/api/admin/room`
Lấy danh sách tất cả phòng
```javascript
// Response 200
[
  {
    "id": 1,
    "name": "Ocean View Suite",
    "description": "Luxury suite with ocean view",
    "capacity": 2,
    "roomStatus": "Available",
    "pricePerNight": 250000,
    "category": "Suite",
    "averageStar": 4.9,
    "bookingCount": 15,
    "thumbnailUrl": "https://...",
    "totalRatingCount": 12
  }
]
```

#### 10. **POST** `/api/admin/room`
Thêm phòng mới
```javascript
// Request Body
{
  "name": "Deluxe Room",
  "description": "Beautiful deluxe room",
  "capacity": 2,
  "roomStatus": "Available",
  "pricePerNight": 180000,
  "category": "Deluxe",
  "thumbnailUrl": "https://..."
}

// Response 200
{ }
```

#### 11. **PUT** `/api/admin/room/{roomId}`
Cập nhật thông tin phòng
```javascript
// Request Body
{
  "name": "Updated Room Name",
  "description": "Updated description",
  "capacity": 3,
  "roomStatus": "Available",
  "pricePerNight": 200000,
  "category": "Suite",
  "thumbnailUrl": "https://..."
}

// Response 200
{ }
```

#### 12. **DELETE** `/api/admin/room/{roomId}`
Xóa phòng
```javascript
// Response 200
{ }
```

### **Service Management**

#### 13. **GET** `/api/admin/service`
Lấy danh sách dịch vụ
```javascript
// Response 200
[
  {
    "id": 1,
    "name": "Room Service",
    "category": "Food & Beverage",
    "price": 50000,
    "unit": "Per Order",
    "isActive": true,
    "imageUrl": "https://..."
  }
]
```

#### 14. **POST** `/api/admin/service`
Thêm dịch vụ mới
```javascript
// Request Body
{
  "name": "Spa Service",
  "category": "Wellness",
  "price": 300000,
  "unit": "Per Session",
  "isActive": true,
  "imageUrl": "https://..."
}

// Response 200
{ }
```

#### 15. **PUT** `/api/admin/service/{serviceId}`
Cập nhật dịch vụ
```javascript
// Request Body
{
  "name": "Updated Service",
  "category": "Wellness",
  "price": 350000,
  "unit": "Per Session",
  "isActive": true
}

// Response 200
{ }
```

#### 16. **DELETE** `/api/admin/service/{serviceId}`
Xóa dịch vụ
```javascript
// Response 200
{ }
```

### **Notification Management (Admin)**

#### 17. **GET** `/api/admin/notification`
Lấy tất cả thông báo
```javascript
// Response 200
[
  {
    "id": 1,
    "content": "System notification",
    "createdAt": "2026-04-10T10:00:00"
  }
]
```

#### 18. **POST** `/api/admin/notification`
Gửi thông báo
```javascript
// Request Body
{
  "content": "Important announcement"
}

// Response 200
{ }
```

#### 19. **PUT** `/api/admin/notification/{notificationId}`
Cập nhật thông báo
```javascript
// Request Body
{
  "content": "Updated announcement"
}

// Response 200
{ }
```

#### 20. **DELETE** `/api/admin/notification/{notificationId}`
Xóa thông báo
```javascript
// Response 200
{ }
```

---

---

## 📌 PUBLIC ENDPOINTS (No Auth Required)

### **Room Listing**

#### 21. **GET** `/api/room`
Lấy danh sách tất cả phòng (cho khách duyệt phòng)
```javascript
// Response 200
[
  {
    "id": 1,
    "name": "Ocean View Suite",
    "description": "Luxury suite with ocean view",
    "capacity": 2,
    "category": "Luxury",
    "pricePerNight": 250000,
    "roomStatus": "Available",
    "averageStar": 4.9,
    "bookingCount": 15,
    "totalRatingCount": 12,
    "thumbnailUrl": "https://..."
  }
]
```

#### 22. **GET** `/api/room/{roomId}`
Lấy chi tiết phòng
```javascript
// Response 200
{
  "id": 1,
  "name": "Ocean View Suite",
  "description": "Luxury suite with ocean view",
  "capacity": 2,
  "category": "Luxury",
  "pricePerNight": 250000,
  "roomStatus": "Available",
  "averageStar": 4.9,
  "bookingCount": 15,
  "totalRatingCount": 12,
  "thumbnailUrl": "https://...",
  "roomImages": [
    {
      "id": 1,
      "imageUrl": "https://...",
      "isThumbnail": true,
      "sortOrder": 1
    }
  ]
}
```

**Status:** ✅ **IMPLEMENTED**

---

## 🔷 Error Handling

Common HTTP Status Codes:
- **200** OK ✅
- **400** Bad Request ❌
- **401** Unauthorized ❌
- **403** Forbidden ❌
- **404** Not Found ❌
- **500** Internal Server Error ❌

Error Response format:
```javascript
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "Invalid request parameters"
}
```

---

## 🔷 JavaScript Examples

### Example 1: Login and Save Token
```javascript
async function login(username, password) {
  const response = await fetch('https://localhost:5135/api/auth/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ username, password })
  });
  
  if (response.ok) {
    const data = await response.json();
    localStorage.setItem('token', data.token);
    return data;
  }
  throw new Error('Login failed');
}
```

### Example 2: Make Authenticated Request
```javascript
async function getMyBookings() {
  const token = localStorage.getItem('token');
  
  const response = await fetch('https://localhost:5135/api/customer/booking/me', {
    method: 'GET',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    }
  });
  
  if (response.ok) {
    return await response.json();
  }
  throw new Error('Failed to fetch bookings');
}
```

### Example 3: Create Booking
```javascript
async function createBooking(roomId, numOfPeople, startTime, endTime) {
  const token = localStorage.getItem('token');
  
  const response = await fetch('https://localhost:5135/api/customer/booking', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      roomId,
      numOfPeople,
      startTime,
      endTime,
      note: ''
    })
  });
  
  if (!response.ok) {
    throw new Error('Booking failed');
  }
  return true;
}
```

### Example 4: Create API Service Wrapper
```javascript
class HotelManagerAPI {
  constructor(baseURL = 'https://localhost:5135') {
    this.baseURL = baseURL;
  }
  
  getToken() {
    return localStorage.getItem('token');
  }
  
  async request(endpoint, options = {}) {
    const url = `${this.baseURL}${endpoint}`;
    const headers = {
      'Content-Type': 'application/json',
      ...options.headers
    };
    
    if (this.getToken()) {
      headers['Authorization'] = `Bearer ${this.getToken()}`;
    }
    
    const response = await fetch(url, {
      ...options,
      headers
    });
    
    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.detail || 'API Error');
    }
    
    return response.status === 204 ? null : await response.json();
  }
  
  // Auth
  async login(username, password) {
    return this.request('/api/auth/login', {
      method: 'POST',
      body: JSON.stringify({ username, password })
    });
  }
  
  async register(userData) {
    return this.request('/api/auth/register', {
      method: 'POST',
      body: JSON.stringify(userData)
    });
  }
  
  // Customer - Bookings
  async getMyBookings() {
    return this.request('/api/customer/booking/me');
  }
  
  async createBooking(bookingData) {
    return this.request('/api/customer/booking', {
      method: 'POST',
      body: JSON.stringify(bookingData)
    });
  }
  
  // Customer - User
  async getMyInfo() {
    return this.request('/api/customer/user/me');
  }
  
  async updateMyInfo(userData) {
    return this.request('/api/customer/user/me', {
      method: 'PUT',
      body: JSON.stringify(userData)
    });
  }
  
  // Customer - Notifications
  async getMyNotifications() {
    return this.request('/api/customer/notification/me');
  }
  
  async markNotificationAsRead(notificationId) {
    return this.request(`/api/customer/notification/${notificationId}`, {
      method: 'PUT'
    });
  }
  
  // Admin - Rooms
  async getAllRooms() {
    return this.request('/api/admin/room');
  }
  
  async createRoom(roomData) {
    return this.request('/api/admin/room', {
      method: 'POST',
      body: JSON.stringify(roomData)
    });
  }
  
  async updateRoom(roomId, roomData) {
    return this.request(`/api/admin/room/${roomId}`, {
      method: 'PUT',
      body: JSON.stringify(roomData)
    });
  }
  
  async deleteRoom(roomId) {
    return this.request(`/api/admin/room/${roomId}`, {
      method: 'DELETE'
    });
  }
  
  // Admin - Services
  async getAllServices() {
    return this.request('/api/admin/service');
  }
  
  async createService(serviceData) {
    return this.request('/api/admin/service', {
      method: 'POST',
      body: JSON.stringify(serviceData)
    });
  }
  
  async updateService(serviceId, serviceData) {
    return this.request(`/api/admin/service/${serviceId}`, {
      method: 'PUT',
      body: JSON.stringify(serviceData)
    });
  }
  
  async deleteService(serviceId) {
    return this.request(`/api/admin/service/${serviceId}`, {
      method: 'DELETE'
    });
  }
}

// Usage
const api = new HotelManagerAPI();

// Login
const loginResult = await api.login('john_doe', 'password123');
localStorage.setItem('token', loginResult.token);

// Get bookings
const bookings = await api.getMyBookings();
console.log(bookings);

// Create booking
await api.createBooking({
  roomId: 1,
  numOfPeople: 2,
  startTime: '2026-04-20T10:00:00',
  endTime: '2026-04-22T10:00:00'
});
```

---

## 🔷 Swagger Documentation
Access interactive API docs at:
```
https://localhost:5135/swagger/ui
```

