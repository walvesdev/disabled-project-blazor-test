#region

using System.Reflection;
using System.Runtime.Loader;

#endregion

namespace Sindvamb.Web.Shared.Helpers;

public static class AssemblyHelper
{
    public static List<Assembly> LoadAssemblies(string folderPath, SearchOption searchOption) =>
        GetAssemblyFiles(folderPath, searchOption)
            .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
            .ToList();

    public static List<Assembly> LoadSolutionAssemblies(string folderPath, SearchOption searchOption) =>
        GetSolutionAssemblyFiles(folderPath, searchOption)
            .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
            .ToList();

    public static IEnumerable<string> GetAssemblyFiles(string folderPath, SearchOption searchOption) =>
        Directory
            .EnumerateFiles(folderPath, "*.*", searchOption)
            .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe"));

    public static IEnumerable<string> GetSolutionAssemblyFiles(string folderPath, SearchOption searchOption)
    {
        var rootAssembly = Assembly.GetEntryAssembly();
        var solutionName = rootAssembly.FullName.Split(".").FirstOrDefault();

        return Directory
            .EnumerateFiles(folderPath, $"{solutionName}.*", searchOption)
            .Where(s => s.EndsWith(".dll") && s.Contains(solutionName));
    }

    public static IEnumerable<Type?> GetAllTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types;
        }
    }
}
