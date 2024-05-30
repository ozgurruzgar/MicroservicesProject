using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.FakePayment;
using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfo checkoutInfo)
        {
            var basket = await _basketService.Get();

            var paymentInfo = new PaymentInfo
            {
                CardName = checkoutInfo.CardName,
                CardNumber = checkoutInfo.CardNumber,
                CVV = checkoutInfo.CVV,
                Expiration = checkoutInfo.Expiration,
                TotalPrice = basket.TotalPrice
            };

            var responsePayment = await _paymentService.RecievePayment(paymentInfo);
            if (!responsePayment)
                return new OrderCreatedViewModel { Error = "Ödeme Alınamadı", IsSuccessful = false };

            var orderCreateInput = new OrderCreateInput
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressInput
                {
                    Province = checkoutInfo.Province,
                    District = checkoutInfo.District,
                    Line = checkoutInfo.Line,
                    Street = checkoutInfo.Street,
                    ZipCode = checkoutInfo.ZipCode,
                }
            };

            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateInput { ProductId = x.CourseId, Price = x.GetCurrentPrice, ProductName = x.CourseName, PictureUrl = "" };
                orderCreateInput.OrderItems.Add(orderItem);
            });

            var response = await _httpClient.PostAsJsonAsync("orders", orderCreateInput);
            if (!response.IsSuccessStatusCode)
                return new OrderCreatedViewModel { Error = "Ödeme Alınamadı", IsSuccessful = false };

            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();
            orderCreatedViewModel.Data.IsSuccessful = true;
            _basketService.Delete();
            return orderCreatedViewModel.Data;
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");
            return response.Data;
        }

        public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfo checkoutInfo)
        {
            var basket = await _basketService.Get();

            var orderCreateInput = new OrderCreateInput
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressInput
                {
                    Province = checkoutInfo.Province,
                    District = checkoutInfo.District,
                    Line = checkoutInfo.Line,
                    Street = checkoutInfo.Street,
                    ZipCode = checkoutInfo.ZipCode,
                }
            };

            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateInput { ProductId = x.CourseId, Price = x.GetCurrentPrice, ProductName = x.CourseName, PictureUrl = "" };
                orderCreateInput.OrderItems.Add(orderItem);
            });

            var paymentInfo = new PaymentInfo
            {
                CardName = checkoutInfo.CardName,
                CardNumber = checkoutInfo.CardNumber,
                CVV = checkoutInfo.CVV,
                Expiration = checkoutInfo.Expiration,
                TotalPrice = basket.TotalPrice,
                Order = orderCreateInput,
            };

            var responsePayment = await _paymentService.RecievePayment(paymentInfo);
            if (!responsePayment)
                return new OrderSuspendViewModel { Error = "Ödeme Alınamadı", IsSuccessful = false };

            await _basketService.Delete();
            return new OrderSuspendViewModel { IsSuccessful = true };
        }
    }
}
