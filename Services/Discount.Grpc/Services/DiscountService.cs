using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;

namespace Discount.Grpc.Services;

public class DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await repository.GetDiscountAsync(request) ?? new Coupon
            { ProductName = "No Discount", Amount = 0, Description = "No Discount" };

        logger.LogInformation($"Discount for product: {coupon.ProductName}");

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Adapt<Coupon>();

        if (coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        await repository.CreateDiscountAsync(coupon);
        logger.LogInformation($"Discount for product was created: {coupon.ProductName}");

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Adapt<Coupon>();

        if (coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        var result = await repository.GetDiscountAsync(new GetDiscountRequest { ProductName = coupon.ProductName });

        if (result is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Such product not found {coupon.ProductName}"));

        await repository.UpdateDiscountAsync(coupon);
        logger.LogInformation($"Successfully updated: {coupon.ProductName}");

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        var coupon = await repository.GetDiscountAsync(new GetDiscountRequest { ProductName = request.ProductName });

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Product not found"));

        await repository.DeleteDiscountAsync(coupon);

        logger.LogInformation($"Successfully deleted: {coupon.ProductName}");

        return new DeleteDiscountResponse { Success = true };
    }
}