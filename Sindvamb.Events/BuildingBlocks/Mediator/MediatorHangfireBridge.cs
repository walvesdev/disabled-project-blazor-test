#region

using System.ComponentModel;

#endregion

namespace Sindvamb.Events.BuildingBlocks.Mediator;

public class MediatorHangfireBridge
{
    private readonly IMediatorHandler _handler;

    public MediatorHangfireBridge(IMediatorHandler handler)
    {
        _handler = handler;
    }

    public async Task Send(IRequest command)
    {
        if (await _handler.IsValid(command))
        {
            await _handler.Mediator.Send(command);
        }
        else
        {
            throw new Exception(new ErrorLogModel
            {
                UserMessage = "Houve erros de validação, verifique os dados e tente novamente!",
                Data = command.Serialize()
            }.Serialize());
        }
    }

    [DisplayName("{0}")]
    public async Task Send(string jobName, IRequest command)
    {
        if (await _handler.IsValid(command))
        {
            await _handler.Mediator.Send(command);
        }
        else
        {
            throw new Exception(new ErrorLogModel
            {
                UserMessage = "Houve erros de validação, verifique os dados e tente novamente!",
                Data = command.Serialize()
            }.Serialize());
        }
    }

    public async Task Send<TCommand>(IRequest<TCommand> command)
    {
        if (await _handler.IsValid(command))
        {
            await _handler.Mediator.Send(command);
        }
        else
        {
            throw new Exception(new ErrorLogModel
            {
                UserMessage = "Houve erros de validação, verifique os dados e tente novamente!",
                Data = command.Serialize()
            }.Serialize());
        }
    }

    [DisplayName("{0}")]
    public async Task Send<TCommand>(string jobName, IRequest<TCommand> command)
    {
        if (await _handler.IsValid(command))
        {
            await _handler.Mediator.Send(command);
        }
        else
        {
            throw new Exception(new ErrorLogModel
            {
                UserMessage = "Houve erros de validação, verifique os dados e tente novamente!",
                Data = command.Serialize()
            }.Serialize());
        }
    }
}
