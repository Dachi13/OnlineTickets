using System.Data;
using Dapper;
using Discount.Grpc.Models;
using Shared;

namespace Discount.Grpc.Services;

public class DiscountRepository(DapperContext context) : IDiscountRepository
{
    public async Task<Coupon?> GetDiscountAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        var connection = await context.CreateConnectionAsync();

        string query =
            $"SELECT * FROM public.\"Discounts\" WHERE \"CategoryId\" = {categoryId} AND \"IsDeleted\" = FALSE LIMIT 1";

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(query);

        return coupon;
    }

    public async Task<long> CreateDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default)
    {
        var connection = await context.CreateConnectionAsync();

        var parameters = new DynamicParameters();
        parameters.Add("p_categoryid", coupon.CategoryId, DbType.Int64);
        parameters.Add("p_description", coupon.Description, DbType.String);
        parameters.Add("p_amount", coupon.Amount, DbType.Int32);
        parameters.Add("discountid", dbType: DbType.Int64, direction: ParameterDirection.Output);

        await connection.ExecuteAsync("public.spadddiscount", parameters, commandType: CommandType.StoredProcedure);

        var discountId = parameters.Get<long>("discountid");

        return discountId;
    }

    public Task<Coupon> UpdateDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Coupon> DeleteDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}