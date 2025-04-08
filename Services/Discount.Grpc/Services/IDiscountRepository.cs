using Discount.Grpc.Models;

namespace Discount.Grpc.Services;

public interface IDiscountRepository
{
    Task<Coupon?> GetDiscountAsync(long categoryId, CancellationToken cancellationToken = default);    
    Task<long> CreateDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default);    
    Task<Coupon> UpdateDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default);    
    Task<Coupon> DeleteDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default);    
}