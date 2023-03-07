using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.IRepository;
using PaymentAPI.Models;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IStripeAppService _stripeAppService;
        public StripeController(IStripeAppService stripeAppService)
        {
            this._stripeAppService = stripeAppService;
        }

        [HttpPost("customer/add")]
        public async Task<ActionResult<StripeCustomer>> AddStripeCustomer(
          [FromBody] AddStripeCustomer customer,
          CancellationToken ct)
        {
            StripeCustomer createdCustomer = await this._stripeAppService.AddStripeCustomerAsync(
                customer,
                ct);

            return StatusCode(StatusCodes.Status200OK, createdCustomer);
        }
        [HttpPost("payment/add")]
        public async Task<ActionResult<StripePayment>> AddStripePayment(
           [FromBody] AddStripePayment payment,
           CancellationToken ct)
        {
            StripePayment createdPayment = await  this._stripeAppService.AddStripePaymentAsync(
                payment,
                ct);

            return StatusCode(StatusCodes.Status200OK, createdPayment);
        }
    }
}
