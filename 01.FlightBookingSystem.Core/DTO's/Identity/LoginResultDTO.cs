using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.DTO_s.Identity
{
    public record LoginResultDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }

}
