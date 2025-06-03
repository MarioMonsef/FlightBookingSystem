using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.Interfaces
{
    public interface ISeatNotifier
    {
       
            Task NotifySeatUpdate(int seatId, bool isBooked);
        
    }
}
