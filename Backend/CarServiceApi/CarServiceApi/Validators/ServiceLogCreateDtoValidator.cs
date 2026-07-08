using CarServiceApi.DTOs;
using FluentValidation;

namespace CarServiceApi.Validators
{
    public class ServiceLogCreateDtoValidator : AbstractValidator<ServiceLogCreateDto>
    {
        public ServiceLogCreateDtoValidator()
        {
            RuleFor(x => x.VehicleId)
                .GreaterThan(0).WithMessage("Valid Vehicle ID is required!");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required!");

            RuleFor(x => x.CarKmCount)
                .InclusiveBetween(0, 2000000).WithMessage("Invalid odometer reading!");

            RuleFor(x => x.ServiceDescription)
                .NotEmpty().WithMessage("Service description is required!")
                .MaximumLength(500).WithMessage("The description is too long (maximum 500 characters).");

            RuleFor(x => x.ServiceCost)
                .InclusiveBetween(0, 10000000).WithMessage("Invalid cost amount!");
        }
    }
}