using System.ComponentModel;

namespace Sindvamb.Domain.Enums;

public enum EUserStatus
{
    [Description("Ativo")]
    ACTIVE = 1,

    [Description("Inativo")]
    INACTIVE = 2,

    [Description("Bloqueado")]
    BLOCKED = 3,

    [Description("Aguardando Aprovação/Liberação")]
    WAITING = 4,

    [Description("Pendente de Ativação")]
    ACTIVATION_PENDING = 5,

    [Description("Troca de Senha Requerida")]
    CHANGE_PASSWORD_NEEDED = 6,
}
