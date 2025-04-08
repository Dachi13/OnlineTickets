namespace Basket.Api.Basket.Create;

public interface IStoreBasketRepository
{
    Task<Result<long?>> StoreToBasket(EventsBasket basket);
    Task<Result<long?>> CachedStoreToBasket(EventsBasket basket);
}