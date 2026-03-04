using HotelManager.Domain.Entity.Rooms;
using HotelManager.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Domain.Entity.RoomImages
{
    public class RoomImage
    {
        protected RoomImage() { }
        public int Id { get; private set; }
        public string ImageUrl { get; private set; }

        // Khóa ngoại trỏ về Room
        public int RoomId { get; private set; }
        public Room Room { get; private set; }
        public bool IsThumbnail { get; private set; }
        public int SortOrder { get; private set; }

        internal RoomImage(string imageUrl, Room room, bool isThumbnail, int sortOrder)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) throw new DomainException("Link ảnh không được trống");

            ImageUrl = imageUrl;
            Room = room;
            IsThumbnail = isThumbnail;
            SortOrder = sortOrder;
        }

        

        internal void SetThumbnail(bool value)
        {
            IsThumbnail = value;
        }
    }
}
