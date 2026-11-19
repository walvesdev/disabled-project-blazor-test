#region

using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using MediatR;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Utils.Filters.CustomExceptions;

#endregion

namespace Sindvamb.HangFire.Jobs;

public abstract class JobBase : IJob
{
    public IMediatorHandler MediatorHandler { get; set; }

    protected JobBase(
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }

    public virtual Task<T> Execute<T>(object? obj, PerformContext? context, IJobCancellationToken cancellationToken)
    {
        throw new CustomException("JobBase Método Execute<T> não Implementado!");
    }

    public virtual Task Execute(object? obj, PerformContext? context, IJobCancellationToken cancellationToken)
    {
        throw new CustomException("JobBase Método Execute não Implementado!");
    }


    #region Exemplo de como utilizar os jobs

    /// <summary>
    ///     Exemplo de como utilizar o Handle para executar os jobs
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    private async Task ExecuteModel(object o, PerformContext context, IJobCancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        context.WriteLine("Iniciando Processo: ");

        await HandleModel(o, context, cancellationToken);

        context.WriteLine("Fim de processamento!");
    }

    /// <summary>
    ///     Exemplo de como utilizar o console do Hangfire para descrever os passos a serem executados
    /// </summary>
    /// <param name="o"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    private Task HandleModel(object o, PerformContext context, IJobCancellationToken cancellationToken)
    {
        var progress = context.WriteProgressBar();
        var list = new List<object>();

        foreach (var obj in list.WithProgress(progress))
        {
            try
            {
                // executa o processo
            }
            catch
            {
                // Muda a cor do texto do console pra representar uma mensagem de erro
                context.SetTextColor(ConsoleTextColor.Red);
                context.WriteLine("Mensagem de erro.");
                context.ResetTextColor();
            }
        }

        context.WriteLine("Registros confirmados:  ");
        context.WriteLine("Registros cancelados:  ");
        return Task.CompletedTask;
    }

    #endregion
}