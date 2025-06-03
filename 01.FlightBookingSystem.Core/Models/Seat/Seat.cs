using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.Models.Seat
{
    public class Seat : EntityBase<int>
    {
        public string SeatNumber { get; set; }

        public bool IsBooking { get; set; } = false;

        public int FlightID { get; set; }
        public virtual Flight.Flight Flight { get; set; }

        public virtual Booking.Booking Booking { get; set; }

        public byte[] Version { get; set; }
    }
}
