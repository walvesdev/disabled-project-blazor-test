using System.ComponentModel;

namespace Sindvamb.Domain.Enums;

public enum EEducationDegree
{
    [Description("Ensino fundamental incompleto")]
    FUNDAMENTAL_INCOMPLETO = 1,

    [Description("Ensino fundamental completo")]
    FUNDAMENTAL_COMPLETO = 2,

    [Description("Ensino médio incompleto")]
    MEDIO_INCOMPLETO = 3,

    [Description("Ensino médio completo")]
    MEDIO_COMPLETO = 4,

    [Description("Graduação incompleta")]
    GRADUACAO_INCOMPLETO = 5,

    [Description("Graduação completa")]
    GRADUACAO_COMPLETO = 6,

    [Description("Pós-graduação")]
    POS_GRADUACAO = 7,
}
