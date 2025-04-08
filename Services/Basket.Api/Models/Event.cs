namespace Basket.Api.Models;

public class Event
{
    public long TicketId { get; set; }
    public int CategoryId { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
}