﻿using FreeCourse.Web.Models.FakePayment;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> RecievePayment(PaymentInfo paymentInfo);
    }
}
