using Dapper;
using FreeCourse.Shared.Dtos;
using Npgsql;
using System.Data;
using System.Security.Cryptography;

namespace FreeCourse.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;
        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> DeleteAsync(int id)
        {
            var status = await _dbConnection.ExecuteAsync("Delete from discount Where id=@Id", new { Id = id });
            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Discount not found", 404);
        }

        public async Task<Response<List<Models.Discount>>> GetAllAsync()
        {
            var discount = await _dbConnection.QueryAsync<Models.Discount>("Select * From discount");
            return Response<List<Models.Discount>>.Success(discount.ToList(), 200);
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserIdAsync(string userId, string code)
        {
            var discount = await _dbConnection.QueryAsync<Models.Discount>("Select * From discount Where userid=@UserId And code=@Code", new { UserId = userId, Code = code });
            var hasDiscount = discount.FirstOrDefault();
            if (hasDiscount == null)
                return Response<Models.Discount>.Fail("Discount not found", 404);
            else
                return Response<Models.Discount>.Success(hasDiscount, 200);
        }

        public async Task<Response<Models.Discount>> GetByIdAsync(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("Select * From discount Where id=@Id", new { Id = id })).SingleOrDefault();
            if (discount == null)
                return Response<Models.Discount>.Fail("Discount not found.", 404);
            else
                return Response<Models.Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> SaveAsync(Models.Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("Insert Into discount (userid,rate,code) Values (@UserId,@Rate,@Code)", discount);
            if (saveStatus > 0)
                return Response<NoContent>.Success(204);
            else
                return Response<NoContent>.Fail("an error occurred while adding", 500);
        }

        public async Task<Response<NoContent>> UpdateAsync(Models.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("Update discount Set userid=@UserId,rate=@Rate,code=@Code Where id=@Id", new { Id = discount.Id, UserId = discount.UserId, Code = discount.Code, Rate = discount.Rate });
            if (status > 0)
                return Response<NoContent>.Success(204);
            else
                return Response<NoContent>.Fail("Discount not found", 404);

        }
    }
}
