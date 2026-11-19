using System.ComponentModel;

namespace Sindvamb.Domain.Enums
{
    public enum TipoPosicaoEnum
    {
        [Description("Gerencial")]
        GERENCIAL = 1,

        [Description("Especialista (Y)")]
        ESPECIALISTA = 2,

        [Description("Analista")]
        ANALISTA = 3,

        [Description("Assistente")]
        ASSISTENTE = 4,
    }
}
