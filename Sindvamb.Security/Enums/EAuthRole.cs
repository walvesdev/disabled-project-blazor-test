using System.ComponentModel;

namespace Sindvamb.Security.Enums;

public enum EAuthRole
{
    [Description("Suporte")] 
    SUPPORT = 8,
                       
    [Description("Administrador")] 
    ADMIN = 12,

    [Description("Master")]
    MASTER = 13,

    [Description("Cliente")]
    CUSTOMER = 14,
}