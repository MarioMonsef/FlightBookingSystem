using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.DTO_s.Seat
{
    public record GetSeatAndFlightNumberDTO
    {
        public int ID { get; set; }

        public string SeatNumber { get; set; }

        public bool IsBooking { get; set; } 

        public string FlightNumber { get; set; }

    }
}
