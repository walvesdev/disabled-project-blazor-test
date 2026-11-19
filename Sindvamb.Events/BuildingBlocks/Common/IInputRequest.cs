namespace Sindvamb.Events.BuildingBlocks.Common;

// public interface IInpuTCommand
// {
//    public dynamic? Input { get; set; }
// }
public interface IInpuTCommand<TInput>
{
    public new TInput? Input { get; set; }
}

public interface IInputResponse<TResponse>
{
    public TResponse Response { get; set; }
}

public interface IInputQueryResponse<TResponse> : ICommandQuery
{
    public IEnumerable<TResponse> Items { get; set; }
}

public class InputQueryResponse<TResponse> : IInputQueryResponse<TResponse>
{
    protected InputQueryResponse(ICommandQuery query, ICollection<TResponse> list)
    {
        Items = query.ToPagedList(list);
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

    public string? Sorting { get; set; }


    public bool HasPrevious { get; set; }


    public bool HasNext { get; set; }

    public string Filter { get; set; }

    public IEnumerable<TResponse> Items { get; set; }
}

public static class InputQueryExtensions
{
    public static IEnumerable<TResult> ToPagedList<TResult>(this ICommandQuery command, ICollection<TResult> list)
    {
        command.TotalCount = list.Count;
        var maxResultCount = command.MaxResultCount > list.Count ? list.Count : command.MaxResultCount;
        var skipCount = command.SkipCount > 0 ? command.SkipCount : (command.CurrentPage - 1) * command.PageSize;

        var query = list.AsQueryable();

        try
        {
            if (!string.IsNullOrWhiteSpace(command.Sorting))
            {
                if (command.Sorting.ToLower().Contains("asc"))
                {
                    query = query.OrderByProperty(command.Sorting.Replace("asc", "").Trim());
                    //return query.ToList();
                }
                else if (command.Sorting.ToLower().Contains("desc"))
                {
                    query = query.OrderByPropertyDesc(command.Sorting.Replace("desc", "").Trim());
                    //return query.ToList();
                }
                else query = query.OrderByProperty(command.Sorting);
                //return query.ToList();
            }
        }
        catch
        {
            //return query.ToList();
        }

        if (maxResultCount > 0) query = list.Skip(0).Take(maxResultCount).AsQueryable();

        var pageSize = command.PageSize <= list.Count ? command.PageSize : list.Count;

        query = command.CurrentPage <= 0
            ? query.Skip(0).Take(pageSize)
            : query.Skip(skipCount).Take(pageSize);

        return query.ToList();
    }
}
