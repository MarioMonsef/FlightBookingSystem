using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.FlightBookingSystem.Core.DTO_s.Payment
{
    public record ConfirmPaymentRequestDTO
    {
        public string PaymentIntentId { get; set; }
        public string PaymentMethodId { get; set; }
    }

}
