using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindvamb.Web.Shared.Constants
{
    public static class SiteConstants
    {

        public static List<string> UFs = new List<string>
    {
        "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI",
        "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
    };

        public static List<string> Gender = new List<string>
    {
        "Masculino", "Feminino", "Não-binário", "Transgênero", "Cisgênero", "Gênero Fluido", "Agênero"
    };

        public static List<string> Race = new List<string>
    {
        "Branca",
        "Preta",
        "Parda",
        "Amarela",
        "Indígena"
    };

        public static List<string> CivilStatus = new List<string>
    {
        "Solteiro",
        "Casado",
        "Separado",
        "Divorciado",
        "Viúvo"
    };

        public static List<KeyValuePair<string, bool>> YesOrNo = new()
    {
        new("Não", false),
        new("Sim", true),
    };

        public static List<string> Housing = new List<string>
    {
        "Casa própria (financiada ou não)",
        "Alugada",
        "Pensão",
        "Gratuita",
        "Outro"
    };

        public static List<string> TipoDependente = new List<string>
    {
        "Cônjugue ou Companheiro",
        "Pais",
        "Padrasto ou Madrasta",
        "Filho(a)",
        "Neto(a)",
        "Enteado(a)",
        "Pessoa Sob Guarda",
        "Avós"
    };

        public static List<string> SpecialNeeds = new List<string>
    {
        "Física",
        "Cegueira ou baixa visão",
        "Auditiva",
        "Intelectual",
        "Mista",
        "Outros"
    };

        public static List<string> ProfilesContact = new List<string>
    {
        "Empreendedor Ambulante ",
        "Imprensa",
        "GDF",
        "Governo Federal",
        "Político",
        "Confederação",
        "Federação",
        "Sindicato",
        "Fornecedor",
        "Parcerias",
        "ONG´s",
        "Estudante",
        "Outro",
    };

        public static List<string> SubjectsContact = new List<string>
    {
        "Associar ",
        "Informação/dúvida",
        "Denúncia",
        "Elogio",
        "Reclamação",
        "Sugestão",
        "Outros",
    };
    }
}
