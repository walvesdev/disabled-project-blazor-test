#region

#endregion

namespace Sindvamb.Events.BuildingBlocks.Common;

public class GetAllQuery<T> : Query<GetAllQuery<T>, GetAllResponse<T>>
{
    public GetAllQuery()
    {
    }

    public GetAllQuery(GetAllInput request) : base(request)
    {
    }
}
