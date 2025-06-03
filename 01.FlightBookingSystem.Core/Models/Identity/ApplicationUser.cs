using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Booking.Booking> Bookings { get; set; } = new List<Booking.Booking>();
    }
}
