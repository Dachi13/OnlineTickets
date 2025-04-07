namespace Basket.Api.Models;

public class EventsBasket
{
    public List<Event> Events { get; set; } = [];
    public decimal TotalPrice => Events.Sum(events => events.Price);
    public long? DiscountId { get; set; }
}