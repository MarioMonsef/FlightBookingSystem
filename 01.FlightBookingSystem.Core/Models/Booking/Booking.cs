using _01.FlightBookingSystem.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.Models.Booking
{
    public class Booking: EntityBase<int>
    {
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public string StripePaymentIntentId { get; set; }

        public int SeatID { get; set; }
        public virtual Seat.Seat Seat { get; set; }

        public string UserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
