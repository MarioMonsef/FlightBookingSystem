using _01.FlightBookingSystem.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace _03.FlightBookingSystem.API.Hubs
{
    /// <summary>
    /// A service that sends real-time notifications to clients about seat updates using SignalR.
    /// </summary>
    public class SeatNotifier : ISeatNotifier
    {
        private readonly IHubContext<SeatHub> _hubContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatNotifier"/> class.
        /// </summary>
        /// <param name="hubContext">The SignalR hub context for broadcasting messages.</param>
        public SeatNotifier(IHubContext<SeatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Sends a seat update notification to all connected clients.
        /// </summary>
        /// <param name="seatId">The ID of the seat being updated.</param>
        /// <param name="isBooked">A boolean value indicating whether the seat is booked.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task NotifySeatUpdate(int seatId, bool isBooked)
        {
            await _hubContext.Clients.All.SendAsync("SeatUpdated", seatId, isBooked);
        }
    }
}
