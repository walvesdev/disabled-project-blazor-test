
using FluentValidation.Results;
#region


#endregion

namespace Sindvamb.Events.BuildingBlocks;

public abstract class Message : IMessage
{
    private dynamic _aggregatedI;

    protected Message()
    {
        ValidationResult = new ValidationResult();
    }

    public string MessageType => GetType().Name;

    public dynamic AggregateId
    {
        get
        {
            if (_aggregatedI == null) return "No AggregateId";

            return _aggregatedI;
        }
        set => _aggregatedI = value;
    }

    public DateTime Timestamp { get; set; }

    public object Data { get; set; }

    public ValidationResult ValidationResult { get; set; }
}
