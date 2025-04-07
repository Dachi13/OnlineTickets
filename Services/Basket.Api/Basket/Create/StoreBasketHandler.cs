using Basket.Api.Models;

namespace Basket.Api.Basket.Create;

public record StoreToBasketCommand(EventsBasket Basket) : ICommand<StoreToBasketResult>;

public record StoreToBasketResult(long? BasketId);

public class StoreBasketHandler(IStoreBasketRepository repository)
    : ICommandHandler<StoreToBasketCommand, StoreToBasketResult>
{
    public async Task<Result<StoreToBasketResult>> Handle(StoreToBasketCommand command,
        CancellationToken cancellationToken)
    {
        // TODO deduct discount and save discountId

        var basketId = await repository.StoreToBasket(command.Basket);

        return new StoreToBasketResult(basketId.Value);
    }
}