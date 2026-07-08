using CarServiceApi.DTOs;
using FluentValidation;

namespace CarServiceApi.Validators
{
    public class VehicleCreateDtoValidator : AbstractValidator<VehicleCreateDto>
    {
        public VehicleCreateDtoValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Valid User ID is required!");

            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("License plate is required!")
                .MaximumLength(10).WithMessage("License plate is too long.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required!");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model is required!");

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, DateTime.Now.Year + 1).WithMessage("Invalid year!");
        }
    }
}