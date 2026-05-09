// File: src/components/ChatbotWidget.tsx
import { useState, useRef, useEffect } from "react";
import axios from "axios";
import "./ChatbotWidget.css";

interface Room {
  id: number;
  name: string;
  price: number;
  category: string;
  capacity: number;
  isAvailable: boolean;
}

interface ChatMessage {
  id: string;
  text: string;
  sender: "user" | "bot";
  rooms?: Room[];
  timestamp: Date;
}

interface ChatResponse {
  message: string;
  rooms?: Room[];
  isRoomQuery: boolean;
  type: "text" | "room_suggestion" | "error";
  timestamp: string;
}

export const ChatbotWidget = () => {
  const [messages, setMessages] = useState<ChatMessage[]>([
    {
      id: "1",
      text: "Xin chào! 👋 Tôi là trợ lý ảo của khách sạn. Bạn muốn tìm phòng nào? Tôi có thể giúp bạn tìm theo giá hoặc loại phòng.",
      sender: "bot",
      timestamp: new Date()
    }
  ]);
  const [input, setInput] = useState("");
  const [loading, setLoading] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  const handleSendMessage = async () => {
    if (!input.trim()) return;

    const userMessage: ChatMessage = {
      id: Date.now().toString(),
      text: input,
      sender: "user",
      timestamp: new Date()
    };

    setMessages((prev: ChatMessage[]) => [...prev, userMessage]);
    setInput("");
    setLoading(true);

    try {
      const token = localStorage.getItem("token");
      const response = await axios.post<ChatResponse>(
        "http://localhost:5135/api/chatbot/message",
        { message: input },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
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

      setMessages((prev: ChatMessage[]) => [...prev, botMessage]);
    } catch (error) {
      console.error("Lỗi gửi tin nhắn:", error);
      const errorMessage: ChatMessage = {
        id: Date.now().toString(),
        text: "Xin lỗi, có lỗi khi xử lý yêu cầu của bạn. Vui lòng thử lại.",
        sender: "bot",
        timestamp: new Date()
      };
      setMessages((prev: ChatMessage[]) => [...prev, errorMessage]);
    } finally {
      setLoading(false);
    }
  };

  const handleQuickSearch = async (query: string) => {
    setInput(query);
    const userMessage: ChatMessage = {
      id: Date.now().toString(),
      text: query,
      sender: "user",
      timestamp: new Date()
    };
    setMessages((prev: ChatMessage[]) => [...prev, userMessage]);
    setLoading(true);

    try {
      const token = localStorage.getItem("token");
      const response = await axios.post<ChatResponse>(
        "http://localhost:5135/api/chatbot/message",
        { message: query },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
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

      setMessages((prev: ChatMessage[]) => [...prev, botMessage]);
    } catch (error) {
      console.error("Lỗi tìm kiếm:", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      {/* Chatbot Button */}
      <div className="chatbot-button-container">
        <button
          className="chatbot-button"
          onClick={() => setIsOpen(!isOpen)}
          title="Mở trợ lý ảo"
        >
          💬
        </button>
      </div>

      {/* Chatbot Panel */}
      {isOpen && (
        <div className="chatbot-panel">
          <div className="chatbot-header">
            <h3>🤖 Trợ lý tìm phòng</h3>
            <button
              className="close-btn"
              onClick={() => setIsOpen(false)}
              aria-label="Đóng"
            >
              ✕
            </button>
          </div>

          <div className="chatbot-messages">
            {messages.map((msg: ChatMessage) => (
              <div key={msg.id} className={`message message-${msg.sender}`}>
                <div className="message-content">
                  <p className="message-text">{msg.text}</p>

                  {msg.rooms?.map((room: Room) => (
                    <div key={room.id} className="room-card">
                      <h4>{room.name}</h4>
                      <span>{room.category}</span>
                    </div>
                  ))}
                  <span className="message-time">
                    {msg.timestamp.toLocaleTimeString("vi-VN", {
                      hour: "2-digit",
                      minute: "2-digit"
                    })}
                  </span>
                </div>
              </div>
            ))}

            {loading && (
              <div className="message message-bot">
                <div className="message-content">
                  <div className="typing-indicator">
                    <span></span><span></span><span></span>
                  </div>
                </div>
              </div>
            )}

            <div ref={messagesEndRef} />
          </div>

          {/* Quick Searches */}
          {messages.length <= 1 && (
            <div className="quick-searches">
              <p>Tìm kiếm nhanh:</p>
              <button
                onClick={() => handleQuickSearch("Tôi muốn tìm phòng từ 500k đến 1 triệu")}
                className="quick-btn"
              >
                500k - 1 triệu
              </button>
              <button
                onClick={() => handleQuickSearch("Có phòng Deluxe không?")}
                className="quick-btn"
              >
                Phòng Deluxe
              </button>
              <button
                onClick={() => handleQuickSearch("Tìm phòng Suite")}
                className="quick-btn"
              >
                Phòng Suite
              </button>
            </div>
          )}

          <div className="chatbot-input">
            <input
              type="text"
              value={input}
              onChange={(e) => setInput(e.target.value)}
              onKeyPress={(e) => {
                if (e.key === "Enter" && !loading) {
                  handleSendMessage();
                }
              }}
              placeholder="Nhập yêu cầu của bạn..."
              disabled={loading}
            />
            <button
              onClick={handleSendMessage}
              disabled={loading || !input.trim()}
              className="send-btn"
            >
              {loading ? "..." : "Gửi"}
            </button>
          </div>
        </div>
      )}
    </>
  );
};

export default ChatbotWidget;
