using _01.FlightBookingSystem.Core.DTO_s;
using _01.FlightBookingSystem.Core.DTO_s.Seat;
using _01.FlightBookingSystem.Core.Models.Seat;
using _01.FlightBookingSystem.Core.Services;
using _03.FlightBookingSystem.API.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _03.FlightBookingSystem.API.Controllers
{
    public class SeatController : BaseController
    {
        private readonly ISeatService _seatService;

        public SeatController(IMapper mapper, ISeatService seatService) : base(mapper)
        {
            _seatService = seatService;
        }

        // Adds a new seat to the system
        [Authorize(Roles = "Admin")]
        [HttpPost("AddSeat")]
        public async Task<IActionResult> AddSeat(ADDSeatDTO addSeatDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var seat = _mapper.Map<Seat>(addSeatDTO);
                var isAdded = await _seatService.AddSeatAsync(seat);

                if (!isAdded)
                    return BadRequest(new ResponseAPI(400, "Unable to add the seat."));

                var seatDTO = _mapper.Map<SeatDTO>(seat);
                var url = Url.Link("GetSeatByID", new { ID = seat.ID });

                return Created(url, seatDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        // Retrieves a specific seat by its ID
        [HttpGet("GetByID/{ID}", Name = "GetSeatByID")]
        public async Task<IActionResult> GetByID(int ID)
        {
            try
            {
                if (ID <= 0)
                    return BadRequest(new ResponseAPI(400, "ID is invalid!"));

                var seat = await _seatService.GetSeatByIDAsync(ID);

                if (seat == null)
                    return NotFound(new ResponseAPI(404));

                var seatDTO = _mapper.Map<GetSeatAndFlightNumberDTO>(seat);
                return Ok(seatDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        // Deletes a seat by its ID
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteByID/{ID}")]
        public async Task<IActionResult> DeleteByID(int ID)
        {
            try
            {
                if (ID <= 0)
                    return BadRequest(new ResponseAPI(400, "ID is invalid!"));

                var isDeleted = await _seatService.RemoveSeatByIDAsync(ID);

                return isDeleted
                    ? StatusCode(204, new ResponseAPI(204, "Seat is deleted."))
                    : BadRequest(new ResponseAPI(400, "Delete failed."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        // Updates an existing seat
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateSeat")]
        public async Task<IActionResult> UpdateSeat(UpdateSeatDTO seatDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseAPI(400, "Data is not valid!"));
                var seat=await _seatService.GetSeatByIDAsync(seatDTO.ID);
                if (seat == null)
                    return NotFound(new ResponseAPI(404,"Not Found Seat."));

                seat.SeatNumber = seatDTO.SeatNumber;
                seat.IsBooking = seatDTO.IsBooking;

                var isUpdated = await _seatService.UpdateSeatAsync(seat);
                return isUpdated
                    ? StatusCode(204, new ResponseAPI(204, "Update completed."))
                    : BadRequest(new ResponseAPI(400, "Update failed."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        // Retrieves all seats for a specific flight
        [HttpGet("GetAllSeats/{flightId}")]
        public async Task<IActionResult> GetAllSeatsInSpecificFlight([FromRoute] int flightId)
        {
            try
            {
                if (flightId <= 0)
                    return BadRequest(new ResponseAPI(400, "Flight ID is invalid!"));

                var seats = await _seatService.GetAllSeats(flightId);

                if (seats == null || seats.Count == 0)
                    return NotFound(new ResponseAPI(404, "No seats found."));

                var seatDTO = _mapper.Map<List<GetSeatAndFlightNumberDTO>>(seats);
                return Ok(seatDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        // Retrieves all available (not booked) seats for a specific flight
        [HttpGet("GetAvailableSeats/{flightId}")]
        public async Task<IActionResult> GetAvailableSeatsInSpecificFlight([FromRoute] int flightId)
        {
            try
            {
                if (flightId <= 0)
                    return BadRequest(new ResponseAPI(400, "Flight ID is invalid!"));

                var seats = await _seatService.GetAvailableSeats(flightId);

                if (seats == null || seats.Count == 0)
                    return NotFound(new ResponseAPI(404, "No available seats found."));

                var seatDTO = _mapper.Map<List<GetSeatAndFlightNumberDTO>>(seats);
                return Ok(seatDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }
    }
}
