using Newtonsoft.Json;

namespace Sindvamb.Application.Host.SeedData.Models;

public class ContactModel
{
    [JsonProperty("codigo")]
    public string Codigo { get; set; }

    [JsonProperty("razaoSocial")]
    public string RazaoSocial { get; set; }

    [JsonProperty("nomeFantasia")]
    public string NomeFantasia { get; set; }

    [JsonProperty("endereco")]
    public string Endereco { get; set; }

    [JsonProperty("cep")]
    public string Cep { get; set; }

    [JsonProperty("cidade")]
    public string Cidade { get; set; }

    [JsonProperty("telefone")]
    public string Telefone { get; set; }

    [JsonProperty("celular")]
    public string Celular { get; set; }

    [JsonProperty("fax")]
    public string Fax { get; set; }

    [JsonProperty("codigoAtividadePrincipal")]
    public string CodigoAtividadePrincipal { get; set; }

    [JsonProperty("atividadeEconomica")]
    public string AtividadeEconomica { get; set; }

    [JsonProperty("uf")]
    public string Uf { get; set; }

    [JsonProperty("complemento")]
    public string Complemento { get; set; }
}