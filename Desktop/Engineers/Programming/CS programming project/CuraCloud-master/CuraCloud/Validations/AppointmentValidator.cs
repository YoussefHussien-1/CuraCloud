using FluentValidation;
using CuraCloud.DTOS;

namespace CuraCloud.Validations
{
    public class AppointmentValidator : AbstractValidator<CreateAppointmentDto>
    {
        public AppointmentValidator()
        {
            RuleFor(a => a.AppointmentDate)
                .NotEmpty().WithMessage("Appointment date is required")
                .GreaterThan(DateTime.UtcNow).WithMessage("You can not register an appointment in the past.");

            RuleFor(a => a.Fees)
                .GreaterThanOrEqualTo(0).WithMessage("Fees must be +ve value");

            RuleFor(a => a.PatientId)
                .NotEmpty().WithMessage("you should select the patient.");

            RuleFor(a => a.TenantId)
                .NotEmpty().WithMessage("you should select the clinic.");
        }
    }
}