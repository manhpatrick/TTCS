using HotelManager.Domain.Entity.Services.Enum;
using System.ComponentModel.DataAnnotations;

namespace HotelManager.Application.DTO.Services
{
    public class ServiceRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EnumDataType(typeof(CategoryService))]
        public CategoryService Category { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string? ImageUrl { get; set; }
    }
}
