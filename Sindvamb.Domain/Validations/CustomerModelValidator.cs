using FluentValidation;

using Sindvamb.Domain.Models;

namespace Sindvamb.Domain.Validations
{
    //public class CustomerModelValidator : AbstractValidator<CustomerModel>
    //{
    //    public CustomerModelValidator()
    //    {
    //        RuleFor(x => x.User.UFIssuingBody).NotEmpty().WithMessage("Por favor insira a UF do seu RG");
    //        RuleFor(x => x.User.CpfCnpj).NotEmpty().WithMessage("Por favor insira seu CPF");
    //        RuleFor(x => x.User.CellPhone).NotEmpty().WithMessage("Por favor insira seu Celular");
    //        RuleFor(x => x.User.Birthdate).NotEmpty().WithMessage("Por favor insira sua Data de Nascimento");

    //        RuleFor(x => x.User.Address)
    //            .SetValidator(new AddressDTOValidator());

    //    }
    //}
}
