using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.DTO_s.Booking
{
    public record AddBookingDTO
    {
        [Required]
        public int SeatID { get; set; }

    }
}
