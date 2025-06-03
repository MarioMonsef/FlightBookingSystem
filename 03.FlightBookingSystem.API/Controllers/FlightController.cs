using _01.FlightBookingSystem.Core.DTO_s;
using _01.FlightBookingSystem.Core.DTO_s.Flight;
using _01.FlightBookingSystem.Core.Models.Flight;
using _01.FlightBookingSystem.Core.Services;
using _03.FlightBookingSystem.API.Controllers;
using _03.FlightBookingSystem.API.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class FlightController : BaseController
{
    private readonly IFlightService _flightService;

    public FlightController(IMapper mapper, IFlightService flightService) : base(mapper)
    {
        _flightService = flightService;
    }

    // Get all flights (past and upcoming)
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllFlights()
    {
        try
        {
            var flights = await _flightService.GetAllFlights();

            if (flights == null || flights.Count == 0)
                return NotFound(new ResponseAPI(404, "No flights found."));

            var flightsDTO = _mapper.Map<List<FlightWithListOfSeatDTO>>(flights);
            return Ok(flightsDTO);
        }
        catch (Exception ex)
        {
            return StatusCode( 500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
        }
    }

    // Get only upcoming (future) flights
    [HttpGet("GetUpcomingFlights")]
    public async Task<IActionResult> GetUpcomingFlights()
    {
        try
        {
            var flights = await _flightService.GetUpcomingFlightsAsync();

            if (flights == null || flights.Count == 0)
                return NotFound(new ResponseAPI(404, "No upcoming flights found."));

            var flightsDTO = _mapper.Map<List<FlightWithListOfSeatDTO>>(flights);
            return Ok(flightsDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
        }
    }

    // Get a specific flight by its ID
    [HttpGet("GetByID/{ID}", Name = "GetFlightByID")]
    public async Task<IActionResult> GetFlightByID(int ID)
    {
        try
        {
            if (ID <= 0)
                return BadRequest(new ResponseAPI(400, "Invalid ID."));

            var flight = await _flightService.GetFlightByID(ID);

            if (flight == null)
                return NotFound(new ResponseAPI(404, "Flight not found."));

            var flightDTO = _mapper.Map<FlightWithListOfSeatDTO>(flight);
            return Ok(flightDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
        }
    }

    // Add a new flight
    [Authorize(Roles = "Admin")]
    [HttpPost("AddFlight")]
    public async Task<IActionResult> AddFlight(ADDFlightDTO flightDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseAPI(400, "Invalid flight data."));

            var flight = _mapper.Map<Flight>(flightDTO);
            var isAdded = await _flightService.AddFlight(flight);

            if (!isAdded)
                return BadRequest(new ResponseAPI(400, "Unable to add the flight."));

            var url = Url.Link(nameof(GetFlightByID), new { ID = flight.ID });
            return Created(url, flight);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
        }
    }

    // Update an existing flight
    [Authorize(Roles =" Admin")]
    [HttpPut("UpdateFlight")]
    public async Task<IActionResult> UpdateFlight([FromForm] UpdateFlightDTO flightDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseAPI(400, "Invalid flight data."));

            var flight = await _flightService.GetFlightByID(flightDTO.ID);
            if (flight == null)
                return NotFound(new ResponseAPI(404, "Not Found Flight."));

            flight.FlightNumber = flightDTO.FlightNumber;
            flight.ArrivalTime = flightDTO.ArrivalTime;
            flight.DepartureTime = flightDTO.DepartureTime;
            flight.Price = flightDTO.Price;
            flight.ArrivalCity = flightDTO.ArrivalCity;
            flight.DepartureCity = flightDTO.DepartureCity;

            var isUpdated = await _flightService.UpdateFlight(flight);
            return isUpdated
                ? StatusCode(204, new ResponseAPI(204, "Flight updated successfully."))
                : BadRequest(new ResponseAPI(400, "Update failed: Flight not found or invalid data."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
        }
    }

    // Delete a flight by ID
    [Authorize(Roles ="Admin")]
    [HttpDelete("DeleteFlight/{ID}")]
    public async Task<IActionResult> RemoveFlightAsync(int ID)
    {
        try
        {
            if (ID <= 0)
                return BadRequest(new ResponseAPI(400, "Invalid ID."));

            var isRemoved = await _flightService.RemoveFlight(ID);

            return isRemoved
                ? Ok(new ResponseAPI(200, "Flight deleted successfully."))
                : NotFound(new ResponseAPI(404, "Flight not found."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
        }
    }
}
