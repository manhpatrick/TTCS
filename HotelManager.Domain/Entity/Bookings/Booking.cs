using HotelManager.Domain.Entity.Accounts;
using HotelManager.Domain.Entity.Bookings.Enum;
using HotelManager.Domain.Entity.Ratings;
using HotelManager.Domain.Entity.Rooms;
using HotelManager.Domain.Entity.ServiceUsages;
using HotelManager.Domain.Exceptions;

namespace HotelManager.Domain.Entity.Bookings
{
    public class Booking
    {
        protected Booking() { }
        public int Id { get; private set; }
        public string? Note { get; private set; }
        public int NumOfPeople { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public decimal RoomPriceAtBooking { get; private set; }
        public BookingStatus BookingStatus { get; private set; } = BookingStatus.Pending;
        public DateTime? ApprovedAt { get; private set; }
        public int AccountId { get; private set; }
        public Account Account { get; private set; }
        public int RoomId { get; private set; }
        public Room? Room { get; private set; }
        public Rating? Rating { get; private set; }
        public decimal TotalPrice { get; private set; }
        
        private readonly List<ServiceUsage> _serviceUsage = new();
        public IReadOnlyCollection<ServiceUsage> ServiceUsages => _serviceUsage;

        public Booking(int accountId, int roomId, string? note, DateTime startTime, DateTime endTime, int numOfPeople, decimal roomPriceAtBooking)
        {
            AccountId = accountId;
            
            RoomId = roomId;
            ChangeNote(note);
            SetStartTime(startTime);
            SetEndTime(endTime);           
            ChangeNumOfPeople(numOfPeople);
            SetRoomPrice(roomPriceAtBooking);
            CreatedAt = DateTime.UtcNow;
        }
        public void ChangeNumOfPeople(int newNumOfPeople)
        {
            if (newNumOfPeople <= 0) throw new DomainException("Số người phải lớn hơn 0");
            NumOfPeople = newNumOfPeople;
        }
        public void ChangeNote(string newNote)
        {
            Note = newNote;
        }
        public void ChangeRoom(Room room)
        {
            Room = room;
        }
        public void SetStartTime(DateTime newStartTime)
        {
            if (EndTime != default && newStartTime > EndTime)
                throw new DomainException("Ngày bắt đầu phải nhỏ hơn ngày kết thúc");
            StartTime = newStartTime;
        }
        public void SetEndTime(DateTime newEndTime)
        {
            if (newEndTime < StartTime) throw new DomainException("Ngày kết thúc phải lớn hơn ngày bắt đầu");
            EndTime = newEndTime;
        }
        public void SetApproved(BookingStatus newBookingStatus)
        {
            BookingStatus = newBookingStatus;
            ApprovedAt = DateTime.UtcNow;
        }
        private void SetRoomPrice(decimal price)
        {
            if (price <= 0)
                throw new DomainException("Giá phòng không hợp lệ");
            RoomPriceAtBooking = price;
        }


        public void AddService(int serviceId, int quantity, decimal unitPrice)
        {
            var existingService = _serviceUsage.FirstOrDefault(x => x.ServiceId == serviceId);
            if (existingService != null)
            {
                // Cách 1: Báo lỗi như bạn làm
                throw new DomainException("Dịch vụ này đã được thêm vào phòng.");
            }

            var bookingService = new ServiceUsage(serviceId ,quantity, unitPrice);
            _serviceUsage.Add(bookingService);

            UpdateTotalPrice();
        }

        public void RemoveService(int serviceId)
        {
            var service = _serviceUsage.FirstOrDefault(x => x.ServiceId == serviceId);
            if (service == null) return;

            _serviceUsage.Remove(service);

            // 🔥 Xóa xong cũng phải tính lại tiền
            UpdateTotalPrice();
        }

        // Đổi tên hàm cho rõ nghĩa và bỏ tham số (vì class đã tự lưu giá phòng rồi)
        private void UpdateTotalPrice()
        {
            // 1. Tính tiền phòng (Dựa trên giá snapshot)
            var nights = CalculateNights();
            var roomTotal = nights * RoomPriceAtBooking;

            // 2. Tính tiền dịch vụ
            var serviceTotal = _serviceUsage.Sum(x => x.TotalPrice);

            // 3. Tổng cộng
            TotalPrice = roomTotal + serviceTotal;
        }
        public int CalculateNights()
        {
            var nights = (EndTime.Date - StartTime.Date).Days;
            return nights <= 0 ? 1 : nights;
        }
        public int count()
        {
            return _serviceUsage.Count;

        }
        public void AddRating(int numOfRating, string review)
        {
            Rating = new Rating(this, numOfRating, review);
        }
    }
}
