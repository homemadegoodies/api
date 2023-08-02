using Microsoft.Extensions.Configuration;
using Stripe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class StripeService
    {
        public StripeService()
        {
            DotNetEnv.Env.Load(); // Load the .env file
            var secretKey = DotNetEnv.Env.GetString("Stripe__SecretKey");
            StripeConfiguration.ApiKey = secretKey;
        }

        public async Task<PaymentIntent> CreatePaymentIntent(double amount)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // Stripe amount is in cents
                Currency = "CAD", // Replace with your desired currency code
                PaymentMethodTypes = new List<string> { "card" } // Accept card payments
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return paymentIntent;
        }
    }
}

