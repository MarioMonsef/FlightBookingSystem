using _01.FlightBookingSystem.Core.Models.Seat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.Models.Flight
{
    public class Flight : EntityBase<int>
    {
        public string FlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }

        public ICollection<Seat.Seat> Seats { get; set; } = new List<Seat.Seat>();
    }
}
