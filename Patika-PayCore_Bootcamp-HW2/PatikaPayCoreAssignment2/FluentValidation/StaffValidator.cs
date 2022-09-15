using FluentValidation;
using FluentValidation.AspNetCore;
using PatikaPayCoreAssignment2.Entity;

namespace PatikaPayCoreAssignment2.FluentValidation
{
    //Staff nesnesine ait doğrulama sınıfı
    public class StaffValidator: AbstractValidator<Staff>
    {
        public StaffValidator()
        {
            RuleFor(s=>s.Id).NotEmpty().GreaterThan(0);
            RuleFor(s => s.Id).GreaterThan(0);

            RuleFor(s => s.Name).NotEmpty().MinimumLength(5).MaximumLength(250);

            RuleFor(s => s.LastName).NotEmpty().MinimumLength(5).MaximumLength(250);

            RuleFor(s => s.Email).NotEmpty().EmailAddress().Matches(@"^[a-zA-Z\.@]{5,250}$");

            RuleFor(s => s.DateOfBirth).NotEmpty().InclusiveBetween(new(1945, 11, 11), new(2002, 10, 10));

            RuleFor(s => s.PhoneNumber).Matches(@"^[\+]?90[\p{L}\p{N}]+$");

            RuleFor(s => s.Salary).NotEmpty().GreaterThan(2000).LessThan(9000);

        }
    }
}
