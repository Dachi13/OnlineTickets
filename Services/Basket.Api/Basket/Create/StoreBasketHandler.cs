namespace Basket.Api.Basket.Create;

public record StoreToBasketCommand(EventsBasket Basket) : ICommand<StoreToBasketResult>;

public record StoreToBasketResult(long? BasketId);

public class StoreBasketHandler(
    IStoreBasketRepository repository,
    DiscountProtoService.DiscountProtoServiceClient discountServiceClient)
    : ICommandHandler<StoreToBasketCommand, StoreToBasketResult>
{
    public async Task<Result<StoreToBasketResult>> Handle(StoreToBasketCommand command,
        CancellationToken cancellationToken)
    {
        await DeductDiscount(command.Basket);

        var basketId = await repository.CachedStoreToBasket(command.Basket);

        return new StoreToBasketResult(basketId.Value);
    }

    private async Task DeductDiscount(EventsBasket basket)
    {
        foreach (var @event in basket.Events)
        {
            var coupon = await discountServiceClient.GetDiscountAsync(new GetDiscountRequest
                { CategoryId = @event.CategoryId });
            @event.Price -= coupon.Amount;
            if (coupon.Id != 0) @event.SetDiscountId(coupon.Id);
        }
    }
}