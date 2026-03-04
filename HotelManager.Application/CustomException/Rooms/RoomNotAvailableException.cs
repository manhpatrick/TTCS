using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Application.CustomException.Rooms
{
    public class RoomNotAvailableException : AppException
    {
        public RoomNotAvailableException(string message) : base(message) { }
    }
}
