using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;
        

        public PaymentsController( IPaymentService paymentService, ILogger<PaymentsController> logger, IConfiguration config)
        {
            _paymentService = paymentService;
            _logger = logger;
            _endpointSecret = config.GetSection("StripeSettings:WhSecret").Value;
        }

        private readonly string _endpointSecret;

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            
            if(basket == null) return BadRequest(new ApiResponse(400, "Problem with your basket"));

            return basket;
        }
//whsec_1c11e2b091fcd9080f24187f65ca5c04717a5001d11f14ed0bfbc3c09eed26a5
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhooks()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _endpointSecret);

                PaymentIntent intent;
                Order order;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    intent = (PaymentIntent) stripeEvent.Data.Object;
                    this._logger.LogInformation("Payment failed:", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    this._logger.LogInformation("Order updated to OrderFailed", order.Id);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    intent = (PaymentIntent) stripeEvent.Data.Object;
                    this._logger.LogInformation("Payment succeeded:", intent.Id);
                    //change order status
                    order = await _paymentService.UpdateOrderPaymentSuccseed(intent.Id);
                    this._logger.LogInformation("Order updated to OrderReceived", order.Id);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

            }
            catch (StripeException e)
            {
                return BadRequest(new ApiResponse(500, "Stripe exception:"+ e.Message));
            }
            return new EmptyResult();
        }
    }
}