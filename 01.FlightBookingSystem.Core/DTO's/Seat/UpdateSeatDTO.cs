using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.DTO_s.Seat
{
    public record UpdateSeatDTO
    {
        [Required]  
        public int ID { get; set; }
        [Required]
        public string SeatNumber { get; set; }
        [Required]
        public bool IsBooking { get; set; } 

    }
}
