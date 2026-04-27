using HotelManager.Domain.Entity.Services.Enum;

namespace HotelManager.Application.DTO.Services
{
    public class ServiceAdminResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryService Category { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }
}
