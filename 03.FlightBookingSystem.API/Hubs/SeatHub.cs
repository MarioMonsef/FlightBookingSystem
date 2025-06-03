using Microsoft.AspNetCore.SignalR;

namespace _03.FlightBookingSystem.API.Hubs
{
    /// <summary>
    /// Hub responsible for managing real-time seat updates via SignalR.
    /// </summary>
    public class SeatHub : Hub
    {
        /// <summary>
        /// Sends a seat update to all connected clients, notifying them whether the seat is booked.
        /// </summary>
        /// <param name="SeatID">The ID of the seat being updated.</param>
        /// <param name="isBooked">A boolean indicating whether the seat is booked or not.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendSeatUpdate(int SeatID, bool isBooked)
        {
            await Clients.All.SendAsync("SeatUpdated", SeatID, isBooked);
        }
    }
}
