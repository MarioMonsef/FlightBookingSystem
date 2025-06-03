using _01.FlightBookingSystem.Core.DTO_s.Identity;
using _01.FlightBookingSystem.Core.Models.Identity;
using AutoMapper;

namespace _03.FlightBookingSystem.API.Mapping
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<ApplicationUser,RegisterDTO>().ReverseMap();
        }
    }
}
