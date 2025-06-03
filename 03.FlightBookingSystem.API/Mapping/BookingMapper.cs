using _01.FlightBookingSystem.Core.DTO_s.Booking;
using _01.FlightBookingSystem.Core.Models.Booking;
using AutoMapper;

namespace _03.FlightBookingSystem.API.Mapping
{
    public class BookingMapper : Profile
    {
        public BookingMapper()
        {
            CreateMap<Booking, BookingDTO>().ForMember(bdto=>bdto.BookingID, option => option.MapFrom(option => option.ID))
                .ForMember(bdto=>bdto.UserName, option => option.MapFrom(option => option.ApplicationUser.UserName))
                .ForMember(bdto=>bdto.SeatNumber, option => option.MapFrom(option => option.Seat.SeatNumber))
                .ReverseMap();
        }
    }
}
