namespace Basket.Api.Basket.Create;

public class StoreToBasketCommandValidator : AbstractValidator<StoreToBasketCommand>
{
    public StoreToBasketCommandValidator()
    {
        RuleFor(basket => basket.Basket).NotNull().NotEmpty().WithMessage("Basket is empty");
        RuleFor(basket => basket.Basket.Events).NotEmpty().WithMessage("Basket is empty");
        RuleForEach(basket => basket.Basket.Events)
            .ChildRules(events =>
            {
                events.RuleFor(e => e.Price)
                    .GreaterThan(0)
                    .WithMessage("Amount must be greater than 0");
            });
    }
}