using HotelManager.Domain.Entity.Rooms.Enum;
using HotelManager.Domain.Entity.Services.Enum;
using HotelManager.Domain.Exceptions;

namespace HotelManager.Domain.Entity.Services
{
    public class Service
    {
        protected Service() { }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public CategoryService Category { get; private set; }
        public decimal Price { get; private set; }
        public string Unit { get; private set; }
        public bool IsActive { get; private set; }
        public string? ImageUrl { get; private set; }
        public Service(string name, decimal price, string unit, CategoryService category)
        {
            Name = name;
            Price = price;
            Unit = unit;
            Category = category;
            IsActive = true;
        }
        public void ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new DomainException("Name is required");
            Name = newName;
        }
        public void ChangeCategoryService(CategoryService newCategory)
        {
            if (!System.Enum.IsDefined(typeof(CategoryService), newCategory)) throw new DomainException("Loại dịch vụ không hợp lệ");
            Category = newCategory;
        }
        public void ChangePrice(decimal newPrice)
        {
            if(newPrice < 0) throw new DomainException("Giá tiền không hợp lệ");
            Price = newPrice;
        }
        public void ChangeUnit(string newUnit)
        {
            if (string.IsNullOrWhiteSpace(newUnit)) throw new DomainException("Unit is required");
            Unit = newUnit;
        }
        public void ChangeIsActive(bool newIsActive)
        {
            IsActive = newIsActive;
        }
        public void ChangeImage(string imageUrl)
        {
            if(string.IsNullOrWhiteSpace(imageUrl)) throw new DomainException("Image is invalid");
            ImageUrl = imageUrl;
        }
    }
}
