using HotelManager.Domain.Entity.Services.Enum;
using System.ComponentModel.DataAnnotations;

namespace HotelManager.Application.DTO.Services
{
    public class ServiceUpdateRequest
    {
        public string? Name { get; set; }

        [EnumDataType(typeof(CategoryService))]
        public CategoryService? Category { get; set; }
        public decimal? Price { get; set; }
        public string? Unit { get; set; }
        public bool? IsActive { get; set; }
        public string? ImageUrl { get; set; }
    }
}
