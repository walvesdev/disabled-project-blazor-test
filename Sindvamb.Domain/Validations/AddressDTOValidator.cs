using FluentValidation;

using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Models;

namespace Sindvamb.Domain.Validations
{
    public class AddressDTOValidator : AbstractValidator<AddressDTO>
    {
        public AddressDTOValidator()
        {

            RuleFor(x => x.ZipCode).NotEmpty().WithMessage("Por favor insira o CEP!");
            RuleFor(x => x.Neighborhood).NotEmpty().WithMessage("Por favor insira o Bairro!");
            RuleFor(x => x.AddressLine1).NotEmpty().WithMessage("Por favor insira o endereço!");
            RuleFor(x => x.City).NotEmpty().WithMessage("Por favor insira a cidade!");
            RuleFor(x => x.District).NotEmpty().WithMessage("Por favor insira o estado!");
            RuleFor(x => x.Housing).NotEmpty().WithMessage("Por favor insira seu tipo de habitação!");

        }
    }
}
