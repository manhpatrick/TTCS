using HotelManager.Domain.Entity.Bookings;
using HotelManager.Domain.Entity.Services;
using HotelManager.Domain.Exceptions;

namespace HotelManager.Domain.Entity.ServiceUsages
{
    public class ServiceUsage
    {
        protected ServiceUsage() { }

        public int Id { get; private set; }

        public int BookingId { get; private set; }
        public Booking Booking { get; private set; } = null!;

        public int ServiceId { get; private set; }
        public Service Service { get; private set; } = null!;

        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; } // giá tại thời điểm chọn

        public decimal TotalPrice { get; private set; }

        public ServiceUsage(int serviceId, int quantity, decimal unitPrice)
        {
            if (quantity < 0) throw new DomainException("Quantity phải > 0");
            if (unitPrice <= 0) throw new DomainException("UnitPrice phải > 0");
            ServiceId = serviceId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = quantity * unitPrice;
        }
    }
}
