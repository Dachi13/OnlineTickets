namespace Basket.Api.Models;

public class EventsBasket
{
    public long? Id { get; set; }
    public long BuyerId { get; set; }
    public List<Event> Events { get; set; } = [];
    public decimal TotalPrice => Events.Sum(events => events.Price * events.Amount);
}