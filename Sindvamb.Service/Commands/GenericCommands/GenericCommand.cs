using Sindvamb.Events.BuildingBlocks.Messages;

namespace Sindvamb.Service.Commands.GenericCommands;


public class GenericCommand<TInput, TResponse> : Command<GenericCommand<TInput, TResponse>, TInput, TResponse>
{
    public GenericCommand(TInput input) => Input = input;
}

public class GenericCommand<TResponse> : Command<GenericCommand<TResponse>, TResponse>
{
}
