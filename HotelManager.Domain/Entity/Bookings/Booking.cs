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
            
            ChangeRoomId(roomId);
            ChangeNote(note);
            SetStartTime(startTime);
            SetEndTime(endTime);           
            ChangeNumOfPeople(numOfPeople);
            SetRoomPrice(roomPriceAtBooking);
            CreatedAt = DateTime.UtcNow;
            UpdateTotalPrice();
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
        public void ChangeRoomId(int roomId)
        {
            RoomId = roomId;
        }
        public void ChangeRoom(Room room)
        {
            Room = room;
        }
        public void SetStartTime(DateTime newStartTime)
        {
            var proposedStartTime = newStartTime.Date.AddHours(12);
            if (EndTime != default && proposedStartTime >= EndTime)
                throw new DomainException("Ngày check-in phải trước ngày check-out ít nhất 1 đêm");
            StartTime = proposedStartTime;

        }
        public void SetEndTime(DateTime newEndTime)
        {
            var proposedEndTime = newEndTime.Date.AddHours(11).AddMinutes(59);
            if (StartTime != default && proposedEndTime <= StartTime)
                throw new DomainException("Ngày check-out phải sau ngày check-in ít nhất 1 đêm");
            EndTime = newEndTime.Date.AddHours(11).AddMinutes(59);
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
            if (BookingStatus != BookingStatus.Completed) throw new DomainException("Không thể đánh giá do chưa hoàn thành booking");
            Rating = new Rating(this, numOfRating, review);
        }
        public void VerifyCanPaid()
        {
            if (BookingStatus != BookingStatus.Pending)
                throw new DomainException("Không thể thanh toán");
        }
        public void MarkAsPaid()
        {   
            BookingStatus = BookingStatus.Confirmed;
            ApprovedAt = DateTime.UtcNow;    
        }
    }
}
