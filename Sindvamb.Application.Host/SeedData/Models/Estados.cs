#region

using Newtonsoft.Json;

namespace Sindvamb.Application.Host.SeedData.Models;



#endregion

public class Mesorregiao
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("nome")] public string Nome { get; set; }

    [JsonProperty("UF")] public UF UF { get; set; }
}

public class Microrregiao
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("nome")] public string Nome { get; set; }

    [JsonProperty("mesorregiao")] public Mesorregiao Mesorregiao { get; set; }
}

public class Regiao
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("sigla")] public string Sigla { get; set; }

    [JsonProperty("nome")] public string Nome { get; set; }
}

public class RegiaoImediata
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("nome")] public string Nome { get; set; }

    [JsonProperty("regiao-intermediaria")] public RegiaoIntermediaria RegiaoIntermediaria { get; set; }
}

public class RegiaoIntermediaria
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("nome")] public string Nome { get; set; }

    [JsonProperty("UF")] public UF UF { get; set; }
}

public class Municipio
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("nome")] public string Nome { get; set; }

    [JsonProperty("microrregiao")] public Microrregiao Microrregiao { get; set; }

    [JsonProperty("regiao-imediata")] public RegiaoImediata RegiaoImediata { get; set; }
}

public class UF
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("sigla")] public string Sigla { get; set; }

    [JsonProperty("nome")] public string Nome { get; set; }

    [JsonProperty("regiao")] public Regiao Regiao { get; set; }
}


public class Estado
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("sigla")] public string Sigla { get; set; }

    [JsonProperty("nome")] public string Nome { get; set; }

    [JsonProperty("regiao")] public Regiao Regiao { get; set; }
}