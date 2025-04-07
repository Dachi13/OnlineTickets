using Discount.Grpc.Models;

namespace Discount.Grpc.Services;

public class DiscountRepository : IDiscountRepository
{
    public Task<Coupon?> GetDiscountAsync(GetDiscountRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Coupon> CreateDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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