namespace Sindvamb.Events.BuildingBlocks.Common;

public class GetAllInput : ICommandQuery
{
    public int CurrentPage { get; set; }

    public int SkipCount { get; set; }

    public int PageSize { get; set; }

    public int MaxResultCount { get; set; }

    public long TotalCount { get; set; }

    public string? Sorting { get; set; }


    public bool HasPrevious { get; set; }


    public bool HasNext { get; set; }

    public string Filter { get; set; }
}
