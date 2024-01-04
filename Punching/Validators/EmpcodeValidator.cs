using DataAccess.Dto.Request;
using FluentValidation;
using System;

namespace Punching.Validators
{
    public class EmpcodeValidator : AbstractValidator<EmpGetReqDto>
    {
        public EmpcodeValidator()
        {
            RuleFor(d => d.empCode).NotNull().GreaterThan(0).WithMessage("EmpID must be greater than 0");
            RuleFor(d => d.empCode).Must(EmpcodeLength).WithMessage("Employee Code Length must be equal or less than 6");
        }

        private bool EmpcodeLength(int empCode)
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
    }
}
