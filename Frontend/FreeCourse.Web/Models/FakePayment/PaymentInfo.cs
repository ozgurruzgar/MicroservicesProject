﻿using FreeCourse.Web.Models.Order;

namespace FreeCourse.Web.Models.FakePayment
{
    public class PaymentInfo
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderCreateInput Order { get; set; }
    }
}
