using FreeCourse.Web.Models.FakePayment;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RecievePayment(PaymentInfo paymentInfo)
        {
            var response = await _httpClient.PostAsJsonAsync("fakepayments", paymentInfo);
            return response.IsSuccessStatusCode;
        }
    }
}
