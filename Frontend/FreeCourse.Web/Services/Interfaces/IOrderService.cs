using FreeCourse.Web.Models.Order;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IOrderService
    {
        // Sync communication - Direct request to order microservice
        Task<OrderCreatedViewModel> CreateOrder(CheckoutInfo checkoutInfo);
        // Async communication - Order informations will send to rabbitMQ
        Task SuspendOrder(CheckoutInfo checkoutInfo);
        Task<List<OrderViewModel>> GetOrder();
    }
}
