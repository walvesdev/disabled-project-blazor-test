using System.IO;
using Sindvamb.Utils.Helpers;
using Sindvamb.Utils.Types;

namespace Sindvamb.Utils.Helpers;

/// <summary>
///     Uma classe auxiliar para operações do Directory.
/// </summary>
public static class DirectoryHelper
{
    public static void CreateIfNotExists(string directory)
    {
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
    }

    public static void DeleteIfExists(string directory)
    {
        if (Directory.Exists(directory)) Directory.Delete(directory);
    }

    public static void DeleteIfExists(string directory, bool recursive)
    {
        if (Directory.Exists(directory)) Directory.Delete(directory, recursive);
    }

    public static void CreateIfNotExists(DirectoryInfo directory)
    {
        if (!directory.Exists) directory.Create();
    }

    public static bool IsSubDirectoryOf(string parentDirectoryPath, string childDirectoryPath)
    {
        Check.NotNull(parentDirectoryPath, "parentDirectoryPath");
        Check.NotNull(childDirectoryPath, "childDirectoryPath");
        return IsSubDirectoryOf(new DirectoryInfo(parentDirectoryPath), new DirectoryInfo(childDirectoryPath));
    }

    public static bool IsSubDirectoryOf(DirectoryInfo parentDirectory, DirectoryInfo childDirectory)
    {
        Check.NotNull(parentDirectory, "parentDirectory");
        Check.NotNull(childDirectory, "childDirectory");
        if (parentDirectory.FullName == childDirectory.FullName) return true;

        var parentOfChild = childDirectory.Parent;
        if (parentOfChild == null) return false;

        return IsSubDirectoryOf(parentDirectory, parentOfChild);
    }

    public static IDisposable ChangeCurrentDirectory(string targetDirectory)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        if (currentDirectory.Equals(targetDirectory, StringComparison.OrdinalIgnoreCase)) return NullDisposable.Instance;

        Directory.SetCurrentDirectory(targetDirectory);
        return new DisposeAction(delegate { Directory.SetCurrentDirectory(currentDirectory); });
    }

    public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
    {
        var directory = new DirectoryInfo(
            currentPath ?? Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles("*.sln").Any()) directory = directory.Parent;

        return directory;
    }

    public static void RenameDirectoryTree(string path, Func<string, string> renamingRule)
    {
        var di = new DirectoryInfo(path);
        RenameDirectoryTree(di, renamingRule);
    }

    public static void RenameDirectoryTree(DirectoryInfo directory, Func<string, string> renamingRule)
    {
        InternalRenameDirectoryTree(directory, renamingRule);

        var currentName = directory.Name;
        var newName = renamingRule(currentName);
        if (currentName != newName)
        {
            var newDirname = Path.Combine(directory.Parent.FullName, newName);
            directory.MoveTo(newDirname);
        }
    }

    private static void InternalRenameDirectoryTree(DirectoryInfo di, Func<string, string> renamingRule)
    {
        foreach (var item in di.GetFileSystemInfos())
        {
            var subdir = item as DirectoryInfo;
            if (subdir != null)
            {
                InternalRenameDirectoryTree(subdir, renamingRule);

                var currentName = subdir.Name;
                var newName = renamingRule(currentName);
                if (currentName != newName)
                {
                    var newDirname = Path.Combine(subdir.Parent.FullName, newName);
                    subdir.MoveTo(newDirname);
                }
            }

            var file = item as FileInfo;
            if (file != null)
            {
                var currentName = Path.GetFileNameWithoutExtension(file.Name);
                var newName = renamingRule(currentName);
                if (currentName != newName)
                {
                    var newFilename = Path.Combine(file.DirectoryName, newName + file.Extension);
                    file.MoveTo(newFilename);
                }
            }
        }
    }

    public static void RenameFileTree(string path, Func<string, string> renamingRule)
    {
        try
        {
            var di = new FileInfo(path);
            RenameFileTree(di, renamingRule);
        }
        catch
        {
        }
    }

    public static void RenameFileTree(FileInfo fileInfo, Func<string, string> renamingRule)
    {
        InternalRenameFileTree(fileInfo, renamingRule);

        var currentName = fileInfo.Name;
        var newName = renamingRule(currentName);
        if (currentName != newName)
        {
            var newDirname = Path.Combine(fileInfo.FullName, newName);
            fileInfo.MoveTo(newDirname);
        }
    }

    private static void InternalRenameFileTree(FileInfo fileInfo, Func<string, string> renamingRule)
    {
        var fi = new DirectoryInfo(fileInfo.FullName).GetFiles("*", SearchOption.AllDirectories);

        foreach (var file in fi)
            if (file != null)
            {
                var currentName = Path.GetFileNameWithoutExtension(file.Name);
                var newName = renamingRule(currentName);

                if (currentName != newName)
                {
                    var newFilename = Path.Combine(file.DirectoryName, newName + file.Extension);
                    file.MoveTo(newFilename);
                }
            }
    }
}
