using Microsoft.Extensions.Configuration;
using Stripe;

namespace _02.FlightBookingSystem.EF.Services
{
    /// <summary>
    /// Service for handling Stripe payment operations like creating, confirming, and refunding payments.
    /// </summary>
    public class PaymentService
    {
        private readonly StripeClient _stripeClient;

        public PaymentService(IConfiguration config)
        {
            var secretKey = config["Stripe:SecretKey"];
            _stripeClient = new StripeClient(secretKey);
        }

        /// <summary>
        /// Creates a Stripe PaymentIntent for a given amount and currency.
        /// </summary>
        /// <param name="amount">The amount to charge (in decimal format).</param>
        /// <param name="currency">The currency to use (default is "usd").</param>
        /// <returns>The created PaymentIntent.</returns>
        public async Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency = "usd")
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" }
            };

            var service = new PaymentIntentService(_stripeClient);
            return await service.CreateAsync(options);
        }

        /// <summary>
        /// Confirms a Stripe PaymentIntent by its ID and associated payment method.
        /// </summary>
        /// <param name="paymentIntentId">The ID of the PaymentIntent to confirm.</param>
        /// <param name="paymentMethodId">The ID of the PaymentMethod to attach and use for confirmation.</param>
        /// <returns>The confirmed PaymentIntent.</returns>
        public async Task<PaymentIntent> ConfirmPaymentIntent(string paymentIntentId,string paymentMethodId)
        {
            var service = new PaymentIntentService(_stripeClient);
            var options = new PaymentIntentConfirmOptions
            {
                CaptureMethod = paymentMethodId,
            };
            return await service.ConfirmAsync(paymentIntentId,options);
        }

        /// <summary>
        /// Creates a refund for a given PaymentIntent.
        /// </summary>
        /// <param name="paymentIntentId">The ID of the PaymentIntent to refund.</param>
        /// <returns>The created Refund object.</returns>
        public async Task<Refund> CreateRefund(string paymentIntentId)
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId,
                Reason = RefundReasons.RequestedByCustomer
            };

            var service = new RefundService(_stripeClient);
            return await service.CreateAsync(options);
        }
    }
}
