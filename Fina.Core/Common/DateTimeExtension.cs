using System.Runtime.InteropServices.JavaScript;

namespace Fina.Core.Common;

//Extension Methods
public static class DateTimeExtension
{
    public static DateTime GetFirstDay(this DateTime date, int? year, int? month) 
        => new(year ?? date.Year, month ?? date.Month, 1);

    public static DateTime GetLastDay(this DateTime date, int? year = null, int? month = null) =>
        new DateTime(
                year ?? date.Year,
                month ?? date.Month,
                1).AddMonths(1)
            .AddDays(-1);
}