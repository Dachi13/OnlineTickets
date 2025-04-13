namespace Events.Api.Events.Create;

public class CreateEventRequestValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventRequestValidator()
    {
        RuleFor(eventCommand => eventCommand.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is Empty")
            .Length(2, 50).WithMessage("Length ({TotalLength}) of {PropertyName} is Invalid");

        RuleFor(eventCommand => eventCommand.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is Empty")
            .Length(2, 1000).WithMessage("Length ({TotalLength}) of {PropertyName} is Invalid");

        // TODO Create a regex that matches correct location format !!!
        RuleFor(eventCommand => eventCommand.Location)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is Empty")
            .Length(2, 1000).WithMessage("Length ({TotalLength}) of {PropertyName} is Invalid");

        RuleFor(eventCommand => eventCommand.CategoryId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is Empty")
            .InclusiveBetween(1, 32_767).WithMessage("Length ({TotalLength}) of {PropertyName} is Invalid");

        RuleFor(eventCommand => eventCommand.AmountOfTickets)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is Empty")
            .GreaterThan(0).WithMessage("{PropertyName} can not be less than 1");        
        
        RuleFor(eventCommand => eventCommand.Price)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is Empty")
            .GreaterThan(0).WithMessage("{PropertyName} can not be less than 1");

        RuleFor(eventCommand => eventCommand.StartTime)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is Empty")
            .Must(StartTimeIsValid).WithMessage("{PropertyName} can not be less than 1");

        RuleFor(eventCommand => eventCommand.EndTime)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is Empty")
            .Must((eventCommand, endDate) => EndTimeIsValid(eventCommand.StartTime, endDate))
            .WithMessage("{PropertyName} must be after StartTime");
    }

    private static bool StartTimeIsValid(DateTime date)
    {
        return date.Date > DateTime.Now.Date;
    }    
    
    private static bool EndTimeIsValid(DateTime startDate, DateTime endDate)
    {
        return startDate < endDate;
    }
}