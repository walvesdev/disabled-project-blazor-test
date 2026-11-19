#region

using System.Text.RegularExpressions;

#endregion

namespace Sindvamb.Utils.Extension;

public static class MethodsExtension
{
    // [Obsolete("Obsolete")]
    // public static void ToPascalNames(this ModelBuilder modelBuilder)
    // {
    //     foreach (var entity in modelBuilder.Model.GetEntityTypes())
    //     {
    //         var tableName = entity.GetTableName().ToPascalCase();
    //         entity.SetTableName(tableName);
    //
    //         foreach (var property in entity.GetProperties())
    //         {
    //             var columnName = ToPascalCase(property.Name);
    //             property.SetColumnName(columnName);
    //         }
    //
    //         foreach (var key in entity.GetKeys())
    //         {
    //             var keyName = key.GetName().ToPascalCase();
    //             key.SetName(keyName);
    //         }
    //
    //         foreach (var key in entity.GetForeignKeys())
    //         {
    //             var foreignKeyName = key.GetConstraintName().ToPascalCase();
    //             key.SetConstraintName(foreignKeyName);
    //         }
    //
    //         foreach (var index in entity.GetIndexes())
    //         {
    //             var indexName = ToPascalCase(index.Name);
    //             index.SetDatabaseName(indexName);
    //         }
    //     }
    // }

    private static string? ToPascalCase(this string? name) =>
        string.IsNullOrWhiteSpace(name)
            ? name
            : Regex.Replace(name, "(?<=[a-z])(?=[A-Z])", "");
}
