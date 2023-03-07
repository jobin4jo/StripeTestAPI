using PaymentAPI.IRepository;
using PaymentAPI.Repository;
using Stripe;

namespace PaymentAPI.commonService
{
    public static class StripeInfrastructure
    {
        public static IServiceCollection AddStripeInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration.GetValue<string>("secretkey");
            return services.AddScoped<CustomerService>()
                .AddScoped<ChargeService>()
                .AddScoped<TokenService>()
                .AddScoped<IStripeAppService, StripeAppService>();
        }
    }
}
