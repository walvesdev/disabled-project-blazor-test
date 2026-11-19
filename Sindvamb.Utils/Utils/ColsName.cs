namespace Sindvamb.Utils.Utils;

public static class ColsName
{
    //C# Excel
    public static string ColumnLabeling(int index)
    {
        var dividend = index;
        var columnName = string.Empty;

        while (dividend > 0)
        {
            var modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo) + columnName;
            dividend = (dividend - modulo) / 26;
        }

        return columnName;
    }
}
