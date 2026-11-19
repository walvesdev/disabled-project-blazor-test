#region

using FluentValidation;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;

#endregion

namespace Sindvamb.Security.Validators.Usuarios;

public class LoginValidator : AbstractValidator<User>
{
    public LoginValidator()
    {
        RuleFor(c => c)
            .NotNull()
            .WithMessage("Por favor, informe a matricula e a senha do usuário!");

        RuleFor(c => c.Email)
            .NotNull()
            .WithMessage("Por favor, informe a matricula do usuário!");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Por favor, informe a senha do usuário!");
    }
}