using CuraCloud.DTOS;
using FluentValidation;

namespace CuraCloud.Validations
{
    public class TenantValidation : AbstractValidator<CreateTenantDtos>
    {
        public TenantValidation()
        {
            RuleFor(tenant => tenant.clinicName)
                .NotEmpty()
                .WithMessage("Clinic Name is required.")
                .Length(5, 50)
                .WithMessage("Lenght is between 5 to 50 letters ");

            RuleFor(tenant => tenant.doctorName)
                .NotEmpty()
                .WithMessage("doctor name is required.")
                .Length(5, 50)
                .WithMessage("doctor name should have greater than 5 letter and less than 50 letters.");

            RuleFor(tenant => tenant.email)
                .EmailAddress()
                .WithMessage("Email is invalid.")
                .When(tenant => !string.IsNullOrEmpty(tenant.email));
            RuleFor(tenant => tenant.DataOfBirth).NotEmpty().WithMessage("the date of birth is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("your date of birth is not valid.");

        }
    }
}
