using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<Response<List<Models.Discount>>> GetAllAsync();
        Task<Response<Models.Discount>> GetByCodeAndUserIdAsync(string code, string userId);
        Task<Response<Models.Discount>> GetByIdAsync(int id);
        Task<Response<NoContent>> SaveAsync(Models.Discount discount);
        Task<Response<NoContent>> UpdateAsync(Models.Discount discount);
        Task<Response<NoContent>> DeleteAsync(int id);

    }
}
