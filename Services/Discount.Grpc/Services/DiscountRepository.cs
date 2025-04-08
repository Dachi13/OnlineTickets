using System.Data;
using Dapper;
using Discount.Grpc.Models;
using Shared;

namespace Discount.Grpc.Services;

public class DiscountRepository(DapperContext context) : IDiscountRepository
{
    public Task<Coupon?> GetDiscountAsync(GetDiscountRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<long> CreateDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default)
    {
        var connection = await context.CreateConnectionAsync();

        var parameters = new DynamicParameters();
        parameters.Add("p_categoryid", coupon.CategoryId, DbType.Int64);  // Use matching type: DbType.Int64 for bigint
        parameters.Add("p_description", coupon.Description, DbType.String);  // Use DbType.String for text
        parameters.Add("p_amount", coupon.Amount, DbType.Int32);  // Use DbType.Decimal for numeric
        parameters.Add("discountid", dbType: DbType.Int64, direction: ParameterDirection.Output);  // Make sure it's an output parameter

        await connection.ExecuteAsync("public.spadddiscount", parameters, commandType: CommandType.StoredProcedure);

// Retrieve the output parameter
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