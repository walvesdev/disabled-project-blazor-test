#region

using System.Diagnostics;
using System.Reflection;

#endregion

namespace Sindvamb.Utils.Extension;

public static class AssemblyExtensions
{
    public static string? GetFileVersion(this Assembly assembly) => FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
}
