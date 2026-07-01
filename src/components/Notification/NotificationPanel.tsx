// File: src/components/NotificationPanel.tsx
import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import "./NotificationPanel.css";

interface Notification {
  id: string;
  title: string;
  message: string;
  timestamp: Date;
  isRead: boolean;
  type?: string;
}

// Hàm phát âm thanh - định nghĩa trước khi sử dụng
const playNotificationSound = () => {
  // Dùng Web Audio API để phát âm thanh
  try {
    const audioContext = new (window.AudioContext || (window as any).webkitAudioContext)();
    const oscillator = audioContext.createOscillator();
    const gainNode = audioContext.createGain();

    oscillator.connect(gainNode);
    gainNode.connect(audioContext.destination);

    oscillator.frequency.value = 1000;
    oscillator.type = "sine";

    gainNode.gain.setValueAtTime(0.3, audioContext.currentTime);
    gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.5);

    oscillator.start(audioContext.currentTime);
    oscillator.stop(audioContext.currentTime + 0.5);
  } catch (e) {
    console.log("Không thể phát âm thanh");
  }
};

export const useNotifications = () => {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    // Tạo SignalR connection
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5135/hubs/notification", {
        accessTokenFactory: () => localStorage.getItem("token") || "",
        transport: signalR.HttpTransportType.WebSockets,
        skipNegotiation: true
      })
      .withAutomaticReconnect([0, 0, 5000, 10000, 30000])
      .build();

    // Xử lý kết nối
    newConnection.onreconnecting(() => {
      console.log("Đang kết nối lại...");
      setIsConnected(false);
    });

    newConnection.onreconnected(() => {
      console.log("Đã kết nối lại!");
      setIsConnected(true);
    });

    // Bắt đầu connection
    newConnection.start()
      .then(() => {
        console.log("Kết nối thành công!");
        setIsConnected(true);
      })
      .catch(err => {
        console.error("Lỗi kết nối:", err);
        setIsConnected(false);
      });

    // Lắng nghe thông báo mới (Đã fix type)
    newConnection.on("ReceiveNotification", (notification: { title?: string; message: string; timestamp: string | number; type?: string }) => {
      const newNotification: Notification = {
        id: Date.now().toString(),
        title: notification.title || "Thông báo mới",
        message: notification.message,
        timestamp: new Date(notification.timestamp),
        isRead: false,
        type: notification.type
      };

      setNotifications(prev => [newNotification, ...prev]);
      setUnreadCount(prev => prev + 1);

      // Phát âm thanh (tùy chọn)
      playNotificationSound();

      // Hiển thị browser notification (tùy chọn)
      if ("Notification" in window && Notification.permission === "granted") {
        new Notification(newNotification.title, {
          body: newNotification.message,
          icon: "🔔"
        });
      }
    });

    // Lắng nghe cập nhật booking (Đã fix type)
    newConnection.on("NewBooking", (booking: { message: string; timestamp: string | number }) => {
      const newNotification: Notification = {
        id: Date.now().toString(),
        title: "📅 Booking mới",
        message: booking.message,
        timestamp: new Date(booking.timestamp),
        isRead: false,
        type: "booking"
      };

      setNotifications(prev => [newNotification, ...prev]);
      setUnreadCount(prev => prev + 1);
      playNotificationSound();
    });

    // Lắng nghe cập nhật thanh toán (Đã fix type)
    newConnection.on("PaymentCompleted", (payment: { message: string; timestamp: string | number }) => {
      const newNotification: Notification = {
        id: Date.now().toString(),
        title: "💳 Thanh toán thành công",
        message: payment.message,
        timestamp: new Date(payment.timestamp),
        isRead: false,
        type: "payment"
      };

      setNotifications(prev => [newNotification, ...prev]);
      setUnreadCount(prev => prev + 1);
      playNotificationSound();
    });

    // Lắng nghe cập nhật badge (Đã fix type)
    newConnection.on("UpdateNotificationBadge", (data: { unreadCount: number }) => {
      setUnreadCount(data.unreadCount);
    });

    setConnection(newConnection);

    return () => {
      newConnection.stop();
    };
  }, []);

  const markAsRead = (id: string) => {
    setNotifications(prev =>
      prev.map(n => n.id === id ? { ...n, isRead: true } : n)
    );
    setUnreadCount(prev => Math.max(0, prev - 1));
  };

  const clearNotifications = () => {
    setNotifications([]);
    setUnreadCount(0);
  };

  const deleteNotification = (id: string) => {
    setNotifications(prev => prev.filter(n => n.id !== id));
    setUnreadCount(prev => Math.max(0, prev - 1));
  };

  return {
    notifications,
    unreadCount,
    connection,
    isConnected,
    markAsRead,
    deleteNotification,
    clearNotifications
  };
};

export const NotificationBell = ({ unreadCount, onClick }: { unreadCount: number; onClick: () => void }) => {
  return (
    <button className="notification-bell" onClick={onClick} title="Thông báo">
      🔔
      {unreadCount > 0 && <span className="notification-badge">{unreadCount > 99 ? "99+" : unreadCount}</span>}
    </button>
  );
};

export const NotificationPanel = () => {
  const { notifications, unreadCount, isConnected, markAsRead, deleteNotification, clearNotifications } = useNotifications();
  const [isOpen, setIsOpen] = useState(false);

  return (
    <>
      <NotificationBell unreadCount={unreadCount} onClick={() => setIsOpen(!isOpen)} />

      {isOpen && (
        <div className="notification-panel">
          <div className="notification-header">
            <h3>🔔 Thông báo ({unreadCount})</h3>
            <div className="notification-controls">
              {unreadCount > 0 && (
                <button
                  className="clear-btn"
                  onClick={clearNotifications}
                  title="Xóa tất cả"
                >
                  ✕
                </button>
              )}
            </div>
          </div>

          <div className="notification-status">
            <span className={`status-indicator ${isConnected ? "connected" : "disconnected"}`}></span>
            {isConnected ? "Kết nối" : "Mất kết nối"}
          </div>

          <div className="notification-list">
            {notifications.length === 0 ? (
              <div className="empty-state">
                <p>📭 Không có thông báo nào</p>
              </div>
            ) : (
              notifications.map(notif => (
                <div
                  key={notif.id}
                  className={`notification-item ${notif.isRead ? "read" : "unread"}`}
                  onClick={() => markAsRead(notif.id)}
                >
                  <div className="notification-content">
                    <h4 className="notification-title">{notif.title}</h4>
                    <p className="notification-message">{notif.message}</p>
                    <span className="notification-time">
                      {notif.timestamp.toLocaleTimeString("vi-VN", {
                        hour: "2-digit",
                        minute: "2-digit"
                      })}
                    </span>
                  </div>
                  <button
                    className="delete-btn"
                    onClick={(e) => {
                      e.stopPropagation();
                      deleteNotification(notif.id);
                    }}
                    title="Xóa"
                  >
                    🗑️
                  </button>
                </div>
              ))
            )}
          </div>
        </div>
      )}
    </>
  );
};

export default NotificationPanel;