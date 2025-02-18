using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Codex.APIs.Errors;
using Store.Codex.Core.Services.Contract;
using Store.Codex.Service.Payments;
using Stripe;

namespace Store.Codex.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<IActionResult> CreatePaymentIntent(string basketId)
        {
            if(basketId is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var basket = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);
            if(basket is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(basket);
        }

        const string endPointSecret = "whsec_9797694e32523bbdb6440a4042110b2c18075a606a72f572da4d07148cdcdfc4";

        [HttpPost("webhook")] // https://localhost:7202/api/payments/webhook
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, endPointSecret);
                Console.WriteLine($"Event received: {stripeEvent.Type}");  // Log event type for debugging

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    Console.WriteLine($"Payment Failed: {paymentIntent.Id}");
                    await _paymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, false);
                }
                else if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    Console.WriteLine($"Payment Succeeded: {paymentIntent.Id}");
                    await _paymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, true);
                }
                else
                {
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                }

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return BadRequest(e);
            }
        }
    }


}
