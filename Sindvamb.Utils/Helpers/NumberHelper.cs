namespace Sindvamb.Utils.Helpers;

internal static class NumberHelper
{
    public static decimal IntegerToDecimal(object value) => Convert.ToDecimal(value) / 100;

    public static double IntegerToDouble(object value) => Convert.ToDouble(value) / 100;

    public static object DecimalToInteger(object value) => Convert.ToInt32(Convert.ToDecimal(value) * 100);

    public static object DoubleToInteger(object value) => Convert.ToInt32(Convert.ToDouble(value) * 100);
}
