using BSNTNext.Application.Dtos.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Validations
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
