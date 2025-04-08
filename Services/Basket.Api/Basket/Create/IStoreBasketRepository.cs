namespace Basket.Api.Basket.Create;

public interface IStoreBasketRepository
{
    Task<Result<long?>> StoreToBasket(EventsBasket basket);
}