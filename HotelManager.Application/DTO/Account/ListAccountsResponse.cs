using HotelManager.Domain.Entity.Users.Enum;

namespace HotelManager.Application.DTO.Account
{
    public class ListAccountsResponse
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public GenderUser? Gender { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public int Role { get; set; }
    }
}
