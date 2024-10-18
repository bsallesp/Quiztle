//// IPaymentsGateway.cs
//public interface IPaymentsGateway
//{
//    #region Plans

//    Task<List<PlanDto>> PopulatePlans(List<PlanDto> plans);
//    Task<List<PlanDto>> GetPlans();

//    #endregion Plans

//    #region Customers

//    Task<bool> CreateCustomer(string name, string email, string systemId);
//    Task<List<CustomerDto>> GetCustomers(int take);
//    Task<CustomerDto> GetCustomerByEmail(string email, params PaymentIncludeDto[] include);
//    Task<CustomerDto> DeleteCustomerByEmail(string email);

//    #endregion Customers

//    #region Payment Methods

//    Task<PaymentMethodIntentModel> PrepareForFuturePayment(string customerId);
//    Task<PaymentMethodIntentModel> PrepareForFuturePaymentWithCustomerEmail(string customerEmail);
//    Task<List<PaymentMethodModel>> GetPaymentMethods(string customerId, PaymentMethodType paymentMethodType);
//    Task<List<PaymentMethodModel>> GetPaymentMethodsByCustomerEmail(string customerEmail, PaymentMethodType paymentMethodType);
//    Task<PaymentMethodModel> AttachPaymentMethod(string paymentMethodId, string customerId, bool makeDefault);
//    Task DeletePaymentMethod(string paymentMethodId);

//    #endregion Payment Methods

//    #region Subscriptions

//    Task<bool> CreateSubscription(string customerEmail, string priceId);

//    #endregion Subscriptions

//    #region Charges

//    Task Charge(string customerId, string paymentMethodId, Currency currency, long unitAmount, string customerEmail, bool sendEmailAfterSuccess = false, string emailDescription = "");
//    Task ChargeWithCustomerEmail(string customerEmail, string paymentMethodId, Currency currency, long unitAmount, bool sendEmailAfterSuccess = false, string emailDescription = "");
//    Task<IEnumerable<ChargeModel>> GetPaymentStatus(string paymentId);

//    #endregion Charges
//}
