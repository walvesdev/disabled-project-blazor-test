namespace Sindvamb.Web.Shared.Services;

public class DragAndDropService
{
    public object Data { get; set; }
    public string Zone { get; set; }
    public string Source { get; set; }

    public void StartDrag(object data, string zone, string source)
    {
        this.Data = data;
        this.Zone = zone;
        this.Source = source;
    }

    public bool Accepts(string zone)
    {
        return this.Zone == zone;
    }
}
