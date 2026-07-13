using CarServiceApi.DTOs;
using FluentValidation;

namespace CarServiceApi.Validators
{
    public class FuelLogCreateDtoValidator : AbstractValidator<FuelLogCreateDto>
    {
        public FuelLogCreateDtoValidator()
        {
            RuleFor(x => x.VehicleId)
                .GreaterThan(0).WithMessage("Valid Vehicle ID is required!");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required!");

            RuleFor(x => x.CarKmCount)
                .InclusiveBetween(0, 2000000).WithMessage("Invalid odometer reading!");

            RuleFor(x => x.FuelAmount)
                .InclusiveBetween(0.1, 200.0).WithMessage("Invalid fuel amount!");

            RuleFor(x => x.FuelCost)
                .InclusiveBetween(0, 1000000).WithMessage("Invalid cost amount!");
        }
    }
}