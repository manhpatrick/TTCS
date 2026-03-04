using HotelManager.Domain.Entity.Accounts.Enum;
using HotelManager.Domain.Entity.Bookings;
using HotelManager.Domain.Entity.Rooms;
using HotelManager.Domain.Entity.Users;
using HotelManager.Domain.Exceptions;

namespace HotelManager.Domain.Entity.Accounts
{
    public class Account
    {
        protected Account() { }
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public RoleAccount Role { get; private set; }
        public bool IsActive { get; private set; }
        public User? User { get; private set; }
        public Account(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new DomainException("Username không được để trống");

            Username = username;
            ChangeRole(RoleAccount.Customer);
            IsActive = true;
            User = new User(this.Id);
        }
        public void ChangePasswordHash(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash)) throw new DomainException("Mật khẩu mã hoá không được trống");
            PasswordHash = newPasswordHash;
        }
        public void ChangeRole(RoleAccount role)
        {
            if (!System.Enum.IsDefined(typeof(RoleAccount), role)) throw new DomainException("Role không hợp lệ");
            Role = role;
        }
        public void ChangeIsActive(bool newIsActive)
        {
            IsActive = newIsActive;
        }
    }
}
