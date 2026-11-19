using Sindvamb.Utils.Extension;

namespace Sindvamb.Application.Core.Attributes;

public class ApiSettingsAttribute : Attribute
{
    public ApiSettingsAttribute(string name = null, string version = "1", bool hidden = false, bool hiddenProduction = false)
    {
        if (!name.IsNullOrWhiteSpace())
        {
            Name = name;
        }

        Version = version;
        Hidden = hidden;
        HiddenProduction = hiddenProduction;
    }

    public string Name { get; set; }

    public bool Hidden { get; set; }

    public bool HiddenProduction { get; set; }

    public string Version { get; set; }

    public bool HiddenHML { get; set; }

    public bool HiddenStaging { get; set; }
}
