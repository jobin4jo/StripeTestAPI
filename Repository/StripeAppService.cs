using PaymentAPI.IRepository;
using PaymentAPI.Models;
using Stripe;

namespace PaymentAPI.Repository
{
    public class StripeAppService : IStripeAppService
    {
        private readonly ChargeService _chargeservice;
        private readonly CustomerService _customerService;
        private readonly TokenService _tokenService;
        public StripeAppService(ChargeService chargeservice, CustomerService customerService, TokenService tokenService)
        {
            this._chargeservice = chargeservice;
            this._customerService = customerService;
            this._tokenService = tokenService;
        }

        public  async Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct)
        {
            TokenCreateOptions tokenoptions = new TokenCreateOptions
            {
                Card= new TokenCardOptions
                {
                    Name = customer.Name,
                    Number = customer.CreditCard.CardNumber,
                    ExpYear = customer.CreditCard.ExpirationYear,
                    ExpMonth = customer.CreditCard.ExpirationMonth,
                    Cvc = customer.CreditCard.Cvc

                }
            };
            Token stripetoken = await this._tokenService.CreateAsync(tokenoptions, null, ct);
            CustomerCreateOptions customeroptions = new CustomerCreateOptions
            {
                Name = customer.Name,
                Email = customer.Email,
                Source = stripetoken.Id
            };
            Customer createdCustomer = await _customerService.CreateAsync(customeroptions, null, ct);
            return new StripeCustomer(createdCustomer.Name,createdCustomer.Email,createdCustomer.Id);
        }

        public async  Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct)
        {
            // Set the options for the payment we would like to create at Stripe
            ChargeCreateOptions paymentOptions = new ChargeCreateOptions
            {
                Customer = payment.CustomerId,
                ReceiptEmail = payment.ReceiptEmail,
                Description = payment.Description,
                Currency = payment.Currency,
                Amount = payment.Amount
            };

            // Create the payment
            var createdPayment = await this._chargeservice.CreateAsync(paymentOptions, null, ct);

            // Return the payment to requesting method
            return new StripePayment(
              createdPayment.CustomerId,
              createdPayment.ReceiptEmail,
              createdPayment.Description,
              createdPayment.Currency,
              createdPayment.Amount,
              createdPayment.Id);
        }
    }
}
