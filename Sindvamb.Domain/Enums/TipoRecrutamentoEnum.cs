using System.ComponentModel;

namespace Sindvamb.Domain.Enums
{
    public enum TipoRecrutamentoEnum
    {
        [Description("Externo")]
        EXTERNO = 1,

        [Description("Interno")]
        INTERNO = 2,

        [Description("Externo ou Interno")]
        EXTERNO_INTERNO = 3,

        [Description("Indicação")]
        INDICACAO = 4,
    }
}
