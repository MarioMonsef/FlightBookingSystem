using _01.FlightBookingSystem.Core.DTO_s.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace _03.FlightBookingSystem.API.Mapping
{
    public class RoleMapper:Profile
    {
        public RoleMapper() {

            CreateMap<IdentityRole,RoleDTO>().ReverseMap();
        }
    }
}
