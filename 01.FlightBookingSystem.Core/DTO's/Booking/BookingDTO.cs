using _01.FlightBookingSystem.Core.Models.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _01.FlightBookingSystem.Core.DTO_s.Booking
{
    public record BookingDTO
    {
        public string UserName { get; set; }
        public string SeatNumber { get; set; }
        public int BookingID { get; set; }
        public DateTime BookingDate { get; set; } 
        public PaymentStatus PaymentStatus { get; set; }
    }
}
