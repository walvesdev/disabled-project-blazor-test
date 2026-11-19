#region

using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.Common;

#endregion

namespace Sindvamb.Service.Inputs;

public class LoginInput : IInpuTCommand<User>
{
    public User Input { get; set; }
}
