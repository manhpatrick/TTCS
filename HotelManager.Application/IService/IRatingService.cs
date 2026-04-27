using HotelManager.Application.DTO.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Application.IService
{
    public interface IRatingService
    {
        Task<IEnumerable<RatingResponse>> GetRoomRatings(int roomId);
    }
}
