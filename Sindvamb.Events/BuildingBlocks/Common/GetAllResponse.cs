namespace Sindvamb.Events.BuildingBlocks.Common;

public class GetAllResponse<T> : InputQueryResponse<T>
{
    public GetAllResponse(ICommandQuery query, ICollection<T> list) : base(query, list)
    {
    }
}
