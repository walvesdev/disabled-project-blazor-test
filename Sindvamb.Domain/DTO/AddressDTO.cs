using Sindvamb.Domain.DTO;

using System.ComponentModel.DataAnnotations;
namespace Sindvamb.Domain.DTO;

public class AddressDTO
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Por favor insira o CEP!")]
    public string? ZipCode { get; set; }


    [Required(ErrorMessage = "Por favor insira o Bairro!")]
    public string? Neighborhood { get; set; }

    [Required(ErrorMessage = "Por favor insira o endereço!")]
    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? Complement { get; set; }

    public long? Number { get; set; }

    [Required(ErrorMessage = "Por favor insira a cidade!")]
    public string? City { get; set; }

    [Required(ErrorMessage = "Por favor insira o estado!")]
    public string? District { get; set; }

    public string? UF { get; set; }

    [Required(ErrorMessage = "Por favor insira seu tipo de habitação!")]
    public string? Housing { get; set; }
}
