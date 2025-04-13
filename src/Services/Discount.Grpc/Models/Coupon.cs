namespace Discount.Grpc.Models;

public class Coupon
{
    public long Id { get; set; }
    public long CategoryId { get; set; }
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
}