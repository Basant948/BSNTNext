using BSNTNext.Application.Dtos.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Validations
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator() 
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Equal(x => x.NewPassword).WithMessage("New password and confirmation do not match.");
        }
    }
}
