using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.DTO_s.Booking
{
    public record BookingResultDTO
    {
        public bool Succeeded { get; set; }
        public string Massage { get; set; }
        public string? ClientSecret { get; set; }
    }
}
