using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Application.CustomException.Auth
{
    public class UsernameNotExistException : AppException
    {
        public UsernameNotExistException(string message) : base(message) { }
    }
}
