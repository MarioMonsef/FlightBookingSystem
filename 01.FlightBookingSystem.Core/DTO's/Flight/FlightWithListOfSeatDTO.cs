using _01.FlightBookingSystem.Core.Models.Seat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.DTO_s
{
    public record FlightWithListOfSeatDTO
    {
        public int ID { get; set; }
        [Required]
        public string FlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        [Range(0,double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        public string DepartureCity { get; set; }
        [Required]
        public string ArrivalCity { get; set; }

        public ICollection<SeatDTO> Seats { get; set; } = new List<SeatDTO>();
    }
}
