#region

using FluentValidation;
using FluentValidation.Results;

#endregion

namespace Sindvamb.Events.BuildingBlocks.Messages;

public abstract class Query<TCommand, TResponse> : Command<TCommand, TResponse>, IQueryMessage<TCommand, TResponse>
    where TCommand : IRequest<TResponse>
{
    protected Query()
    {
        Timestamp = DateTime.UtcNow;
    }

    protected Query(ICommandQuery query)
    {
        Timestamp = DateTime.UtcNow;
        CurrentPage = query.CurrentPage;
        SkipCount = query.SkipCount;
        PageSize = query.PageSize;
        MaxResultCount = query.MaxResultCount;
        TotalCount = query.TotalCount;
        Sorting = query.Sorting;
        Filter = query.Filter;
    }

    public string Sorting { get; set; }

    public int CurrentPage { get; set; }

    public int SkipCount { get; set; }

    public int PageSize { get; set; }

    public int MaxResultCount { get; set; }

    public long TotalCount { get; set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < SkipCount;

    public string Filter { get; set; }

    public object GetMetadataPagination() =>
        new
        {
            MaxResultCount,
            PageSize,
            CurrentPage,
            SkipCount,
            Sorting,
            TotalCount,
            HasNext,
            HasPrevious
        };
}

public abstract class Query<TCommand, TResponse, TValidator, TInput> : IQueryMessage<TCommand, TResponse, TValidator, TInput>
    where TCommand : IBaseRequest, IRequest<TResponse>
    where TInput : class, new()
    where TValidator : AbstractValidator<TCommand>, new()
{
    protected Query(ICommandQuery query)
    {
        Timestamp = DateTime.UtcNow;
        CurrentPage = query.CurrentPage;
        SkipCount = query.SkipCount;
        PageSize = query.PageSize;
        MaxResultCount = query.MaxResultCount;
        TotalCount = query.TotalCount;
        Sorting = query.Sorting;
        Filter = query.Filter;
    }

    public int CurrentPage { get; set; }

    public int SkipCount { get; set; }

    public int PageSize { get; set; }

    public int MaxResultCount { get; set; }

    public long TotalCount { get; set; }

    public string Sorting { get; set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < SkipCount;

    public string Filter { get; set; }

    public string MessageType { get; }

    public dynamic AggregateId { get; set; }

    public DateTime Timestamp { get; set; }

    public object Data { get; set; }

    public ValidationResult ValidationResult { get; set; }

    //public async Task<bool> Validate(IBaseRequest request, IMediatorHandler handler)
    //{
    //    if (request == null)
    //    {
    //        ValidationResult = await new TValidator().ValidateAsync(((TCommand)request)!);
    //    }

    //    if (ValidationResult is null)
    //    {
    //        throw new Exception("Erro ao efetuar validação!");
    //    }

    //    var isValid = ValidationResult.IsValid && ValidationResult.Errors?.Count == 0;

    //    if (!isValid)
    //    {
    //        foreach (var error in ValidationResult.Errors!)
    //        {
    //            await handler.PublishNotification(new DomainNotification(nameof(ValidationResult),
    //                error.ErrorMessage));
    //        }
    //    }

    //    return isValid;
    //}

    public object GetMetadataPagination() =>
        new
        {
            MaxResultCount,
            PageSize,
            CurrentPage,
            SkipCount,
            HasNext,
            HasPrevious
        };

    public dynamic? Input { get; set; }
}

public abstract class QueryHandler<TCommand> : CommandHandler<TCommand>, IQueryHandler<TCommand>
    where TCommand : IRequest
{
    protected QueryHandler(IMediatorHandler mediatorHandler) : base(mediatorHandler)
    {
    }

    public async Task Handle(TCommand request, CancellationToken cancellationToken) => await Execute(request, cancellationToken);
}

public abstract class QueryHandler<TCommand, TResponse> : CommandHandler<TCommand, TResponse>,
    IQueryHandler<TCommand, TResponse>
    where TCommand : IRequest<TResponse>
{
    protected QueryHandler(IMediatorHandler mediatorHandler) : base(mediatorHandler)
    {
    }

    public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken) => await Execute(request, cancellationToken);
}
