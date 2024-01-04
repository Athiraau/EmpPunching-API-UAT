using DataAccess.Dto.Request;
using FluentValidation;

namespace Punching.Validators
{
    public class HostCheckValidator : AbstractValidator<HostCheckReqDto>
    {
        public HostCheckValidator()
        {
            RuleFor(d => d.hostname).NotNull().NotEmpty().WithMessage("Hostname is mandatory");
        }        
    }
}
