using DataAccess.Dto.Request;
using FluentValidation;
using System;

namespace Punching.Validators
{
    public class RetreiveBlobValidator : AbstractValidator<RetreiveBlobReqDto>
    {
        public RetreiveBlobValidator()
        {
            RuleFor(d => d.imgDate).NotNull().NotEmpty().WithMessage("Image selection date is mandatory");
            RuleFor(d => Convert.ToDateTime(d.imgDate)).Must(BeAValidDate).When(x => !string.IsNullOrEmpty(x.imgDate)).WithMessage("Apply date should be a valid date value");
            RuleFor(d => d.empCode).NotNull().NotEmpty().WithMessage("Employee ID is required");
            RuleFor(d => d.empCode).Must(EmpcodeLength).WithMessage("Assigned employee ID Length must be equal or less than 6");
        }

        private bool EmpcodeLength(string empCode)
        {
            if (Convert.ToString(empCode).Length > 6)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

    }
}
