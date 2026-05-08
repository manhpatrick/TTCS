using System;
using System.Collections.Generic;
namespace HotelManager.Application.DTO.Auth
{
    public class LoginResponse
    {
        public string? FullName { get; set; } // Phải có trường này
        public string AccessToken { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string Role { get; set; }
    }
}
