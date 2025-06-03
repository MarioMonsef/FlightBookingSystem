using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.Models
{
    public abstract class EntityBase<T> 
    { 
        public T ID { get; set; }
    }
}
