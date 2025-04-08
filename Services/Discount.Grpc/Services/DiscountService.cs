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
            { CategoryId = 0, Amount = 0, Description = "No Discount" };

        logger.LogInformation($"Discount for product: {coupon.CategoryId}");

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Model.Adapt<Coupon>();

        if (coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        var discountId = await repository.CreateDiscountAsync(coupon);
        logger.LogInformation($"Discount for product was created: {discountId}");

        var couponModel = coupon.Adapt<CouponModel>();

        couponModel.Id = discountId;
        
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Adapt<Coupon>();

        if (coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        var result = await repository.GetDiscountAsync(new GetDiscountRequest { CategoryId = coupon.CategoryId });

        if (result is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Such product not found {coupon.CategoryId}"));

        await repository.UpdateDiscountAsync(coupon);
        logger.LogInformation($"Successfully updated: {coupon.CategoryId}");

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        var coupon = await repository.GetDiscountAsync(new GetDiscountRequest { CategoryId = request.CategoryId });

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Product not found"));

        await repository.DeleteDiscountAsync(coupon);

        logger.LogInformation($"Successfully deleted: {coupon.CategoryId}");

        return new DeleteDiscountResponse { Success = true };
    }
}