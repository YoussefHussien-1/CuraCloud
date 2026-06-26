using CuraCloud.DTOS;
using FluentValidation;
using Microsoft.AspNetCore.Rewrite;
namespace CuraCloud.Validations
{
    public class PatientValidation : AbstractValidator<CreatePatientDtos>
    {
        public PatientValidation()
        {
            RuleFor(patient => patient.FullName).NotEmpty().WithMessage("Full name is required.").Length(3,50).WithMessage("full name lenght must be from 3 to 50 letters");
            RuleFor(patient => patient.phoneNumber)
            .NotEmpty().WithMessage("Phone Number is required.")
            .Matches(@"^01[0125]\d{8}$").WithMessage("It should be an egyption number and include 11 numbers.");
            RuleFor(patient => patient.email).EmailAddress().WithMessage("Email is in valid.").When(p => !string.IsNullOrEmpty(p.email));
            RuleFor(patient => patient.DateOfBirth).NotEmpty().WithMessage("Date of birth is required.").LessThanOrEqualTo(DateTime.Now).WithMessage("You can not enter a date greater than today.");
        }
    }
}
