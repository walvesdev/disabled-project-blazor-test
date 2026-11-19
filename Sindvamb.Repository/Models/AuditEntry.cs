using Microsoft.EntityFrameworkCore.ChangeTracking;

using Newtonsoft.Json;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.Models;

public class AuditEntry
{

    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }

    public EntityEntry Entry { get; }
    public string TableName { get; set; }
    public Guid? UserId { get; set; }
    public string IpAddress { get; set; }
    public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
    public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
    public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
    public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

    public bool HasTemporaryProperties => TemporaryProperties.Any();

    public Audit ToAudit()
    {
        var audit = new Audit();
        audit.TableName = TableName;
        audit.UserId = UserId;
        audit.IpAddress = IpAddress;
        audit.KeyValues = JsonConvert.SerializeObject(KeyValues);
        audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
        audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
        return audit;
    }

}
