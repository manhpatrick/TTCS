using HotelManager.Domain.Entity.Accounts;
using HotelManager.Domain.Entity.Users.Enum;

namespace HotelManager.Domain.Entity.Users
{
    public class User
    {
        protected User() { }
        public int Id { get; private set; }
        public string? Name { get; private set; }
        public GenderUser? Gender { get; private set; }
        public DateOnly? BirthDay { get; private set; }
        public string? Phone { get; private set; }
        public string? Address { get; private set; }
        public string? Email { get; private set; }
        public int AccountId { get; private set; }
        public Account Account { get; private set; }

        public User(int accountId)
        {
            AccountId = accountId;
        }
        public void ChangeName(string newName)
        {
            //if (string.IsNullOrEmpty(newName)) throw new DomainException("Tên không hợp lệ");
            Name = newName.Trim();
        }
        public void ChangeGender(GenderUser newGender)
        {
            //if (!System.Enum.IsDefined(typeof(GenderUser), newGender)) throw new DomainException("Giới tính không hợp lệ");
            Gender = newGender;
        }
        public void ChangeBirthDay(DateOnly newBirthDay)
        {
            //if(newBirthDay > DateOnly.FromDateTime(DateTime.Today)) throw new DomainException("Ngày sinh không hợp lệ");
            BirthDay = newBirthDay;
        }
        public void ChangePhone(string newPhone)
        {
            //if (string.IsNullOrWhiteSpace(newPhone)) throw new DomainException("Số điện thoại không hợp lệ");
            Phone = newPhone.Trim();
        }
        public void ChangeEmail(string newEmail)
        {
            //if (string.IsNullOrWhiteSpace(newEmail)) throw new DomainException("Email không hợp lệ");
            Email = newEmail.Trim();
        }
        public void ChangeAddress(string newAddress)
        {
            //if (string.IsNullOrEmpty(newAddress)) throw new DomainException("Địa chỉ không hợp lệ");
            Address = newAddress.Trim();
        }
    }
}
