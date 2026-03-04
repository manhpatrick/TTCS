using HotelManager.Domain.Entity.Bookings;
using HotelManager.Domain.Entity.RoomImages;
using HotelManager.Domain.Entity.Rooms.Enum;
using HotelManager.Domain.Exceptions;

namespace HotelManager.Domain.Entity.Rooms
{
    public class Room
    {
        protected Room() { }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Capacity { get; private set; }
        public CategoryRoom Category { get; private set; }
        public RoomStatus RoomStatus { get; private set; }
        public decimal PricePerNight { get; private set; }
        public int BookingCount { get; private set; } = 0;
        public decimal AverageStar { get; private set; } = 0;
        public int TotalRatingCount { get; private set; } = 0;
        public string? ThumbnailUrl { get; private set; }

        private readonly List<RoomImage> _roomImages = new();
        public IReadOnlyCollection<RoomImage> RoomImages => _roomImages.AsReadOnly();
        public Room(string roomName, string description, int capacity,CategoryRoom category, RoomStatus roomStatus, decimal pricePerNight)
        {
            ChangeRoomName(roomName);
            ChangeDescription(description);
            ChangeCapacity(capacity);
            ChangeCategoryRoom(category);
            ChangeRoomStatus(roomStatus);
            ChangePricePerNight(pricePerNight);
        }
        public void ChangeCapacity(int newCapacity)
        {
            if (newCapacity < 0) throw new DomainException("Sức chứa không hợp lệ");
            Capacity = newCapacity;
        }
        public void ChangeDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription)) throw new DomainException("Mô tả phòng không được trống");
            Description = newDescription;
        }
        public void ChangeRoomName(string newRoomName)
        {
            if (string.IsNullOrWhiteSpace(newRoomName)) throw new DomainException("Tên phòng không được trống");
            Name = newRoomName;
        }
        public void ChangePricePerNight(decimal newPricePerNight)
        {
            if (newPricePerNight < 0) throw new DomainException("Giá tiền không hợp lệ");
            PricePerNight = newPricePerNight;
        }
        public void ChangeCategoryRoom(CategoryRoom newCategory)
        {
            if (!System.Enum.IsDefined(typeof(CategoryRoom), newCategory)) throw new DomainException("Hạng phòng không hợp lệ");
            Category = newCategory;
        }
        public void ChangeRoomStatus(RoomStatus newRoomStatus)
        {
            if (!System.Enum.IsDefined(typeof(RoomStatus), newRoomStatus)) throw new DomainException("Trạng thái phòng không hợp lệ");
            RoomStatus = newRoomStatus;
        }
        public void AddImage(string imageUrl, bool isThumbnail = false)
        {
            if (isThumbnail)
            {
                foreach (var img in _roomImages)
                    img.SetThumbnail(false);
            }

            var sortOrder = _roomImages.Count + 1;

            var image = new RoomImage(
                imageUrl,
                this,
                isThumbnail || !_roomImages.Any(),
                sortOrder
            );
            ThumbnailUrl = imageUrl;

            _roomImages.Add(image);
        }

        public void RemoveImage(RoomImage image)
        {
            _roomImages.Remove(image);
        }
        public void IncrementBookingCount()
        {
            BookingCount += 1;
        }
        public void ChangeAverageStar(int newStar)
        {
            decimal total = AverageStar * BookingCount + newStar;
            TotalRatingCount += 1;
            AverageStar = Math.Round(total / TotalRatingCount, 1);
        }
    }
}
