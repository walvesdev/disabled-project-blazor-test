using System.ComponentModel;

namespace Sindvamb.Domain.Enums
{
    public enum MotivoAberturaEnum
    {
        [Description("Aumento no quadro de empregados")]
        AUMENTO_QUADRO = 1,

        [Description("Substituição")]
        SUBSTITUICAO = 2,

        [Description("Transferência de empregado")]
        TRANSFERENCIA = 3,
    }
}
