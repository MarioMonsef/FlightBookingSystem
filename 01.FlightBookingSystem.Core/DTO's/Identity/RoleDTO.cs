using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.DTO_s.Identity
{
    public record RoleDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
