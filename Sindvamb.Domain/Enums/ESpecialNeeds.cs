using System.ComponentModel;

namespace Sindvamb.Domain.Enums;

public enum ESpecialNeeds
{
    [Description("Física")]
    FISICA = 1,

    [Description("Cegueira ou baixa visão")]
    CEGUEIRA = 2,

    [Description("Auditiva")]
    AUDITIVA = 3,

    [Description("Intelectual")]
    INTELECTUAL = 4,

    [Description("Mista")]
    MISTA = 5,

    [Description("Outros")]
    OUTROS = 6,
}
