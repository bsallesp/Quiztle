using Stripe;
using Stripe.Checkout;

namespace Quiztle.Frontend.Components.Stripe.Engine
{
    public class StripeBlazor
    {
        private readonly string _domain;
        private readonly string _successPage;
        private readonly string _cancelOrderPage;

        public StripeBlazor(string domain = "https://localhost:7261/", string successPage = "/OrderComplete", string cancelOrderPage = "/OrderAbandoned")
        {
            _domain = domain;
            _successPage = successPage;
            _cancelOrderPage = cancelOrderPage;
        }

        public async Task<string> Checkout(string priceId)
        {
            var sessionUrl = await CreateSession(priceId);
            return sessionUrl;
        }

        private void CreateCostumer(string name, string email)
        {
            var result = SearchCostumer(name, email);
            if (result != null) return;

            var options = new CustomerCreateOptions
            {
                Name = "Jenny Rosen",
                Email = "jennyrosen@example.com",
            };
            var service = new CustomerService();
            service.Create(options);
        }

        public StripeSearchResult<Customer> SearchCostumer(string name, string email)
        {
            var options = new CustomerSearchOptions
            {
                Query = $"name:{name} OR email: {email}"
            };
            var service = new CustomerService();
            StripeSearchResult<Customer> customers = service.Search(options);
            return customers;
        }

        private async Task<string> CreateSession(string priceId)
        {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        Price = priceId,
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = _domain + _successPage,
                CancelUrl = _domain + _cancelOrderPage
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return session.Url;
        }

        public List<Session> GetSessionsList()
        {
            var options = new SessionListOptions { Limit = 30 };
            var service = new SessionService();
            StripeList<Session> sessions = service.List(options);
            return sessions.Data.ToList();
        }
    }
}
