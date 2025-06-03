using _01.FlightBookingSystem.Core.DTO_s;
using _01.FlightBookingSystem.Core.DTO_s.Seat;
using _01.FlightBookingSystem.Core.Models.Seat;
using AutoMapper;

namespace _03.FlightBookingSystem.API.Mapping
{
    public class SeatMapper : Profile
    {
        public SeatMapper()
        {
            CreateMap<Seat,SeatDTO>().ReverseMap();
            CreateMap<Seat,ADDSeatDTO>().ReverseMap();
            CreateMap<Seat, GetSeatAndFlightNumberDTO>()
                .ForMember(s=>s.FlightNumber, op=>
                op.MapFrom(s=>s.Flight.FlightNumber)).ReverseMap();
        }
    }
}
