using _01.FlightBookingSystem.Core.DTO_s;
using _01.FlightBookingSystem.Core.DTO_s.Flight;
using _01.FlightBookingSystem.Core.Models.Flight;
using AutoMapper;

namespace _03.FlightBookingSystem.API.Mapping
{
    public class FlightMapper:Profile
    {
        public FlightMapper()
        {
            CreateMap<Flight,FlightWithListOfSeatDTO>().ReverseMap();
            CreateMap<Flight,ADDFlightDTO>().ReverseMap();
        }
    }
}
