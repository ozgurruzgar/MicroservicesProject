namespace FreeCourse.Web.Models.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        
        //Doesn't take while don't need to address field in payment info 
        //public AddressDto AddressDto { get; set; }
        public string BuyerId { get; set; }

        public List<OrderItemViewModel> OrderItemDtos { get; set; }
    }
}
