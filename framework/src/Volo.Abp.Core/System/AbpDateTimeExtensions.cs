namespace System
{
    /// <summary>
    /// Extension methods for the <see cref="DateTime"/>.
    /// </summary>
    public static class AbpDateTimeExtensions
    {
        public static DateTime ClearTime(this DateTime dateTime)
        {
            return dateTime.Subtract(
                new TimeSpan(
                    0,
                    dateTime.Hour,
                    dateTime.Minute,
                    dateTime.Second,
                    dateTime.Millisecond
                )
            );
        }

        #region 转换方法
        /// <summary>月单位(100)</summary>
        public const int MonthUnit = 100;
        /// <summary>年单位(10000)</summary>
        public const int YearUnit = 10000;
        /// <summary>根据日期值生成整型值</summary>
        public static int ToDay(this DateTime value)
        {
            return ToDay(value.Year, value.Month, value.Day);
        }
        /// <summary>根据日期值生成整型值</summary>
        public static int ToDay(int year, int month, int day)
        {
            return year * YearUnit + month * MonthUnit + day;
        }
        /// <summary>根据整型值生成日期值</summary>
        public static DateTime ToDate(this int value)
        {
            var year = Math.DivRem(value, YearUnit, out var remainder);
            value = remainder;
            var month = Math.DivRem(value, MonthUnit, out remainder);
            return new DateTime(year, month, remainder);
        }
        /// <summary>获得前一天</summary>
        public static int GetPrevDay(this int value)
        {
            var date = value.ToDate();
            var prevDate = date.AddDays(-1);
            return prevDate.ToDay();
        }
        /// <summary>获得后一天</summary>
        public static int GetNextDay(this int value)
        {
            var date = value.ToDate();
            var nextDate = date.AddDays(1);
            return nextDate.ToDay();
        }
        /// <summary>获得两天的日差</summary>
        public static int GetDayDiff(int x, int y)
        {
            var dx = x.ToDate();
            var dy = y.ToDate();
            var diff = dx - dy;
            return (int)diff.TotalDays;
        }
        /// <summary>获得时间对应的那天最后一秒</summary>
        public static DateTime ToDayLastSecond(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
        }
        /// <summary>把时间转换为标准日期时间格式(yyyy/MM/dd HH:mm:ss)</summary>
        public static string ToStandardDateTimeString(this DateTime value, bool treatZeroAsEmpty = false)
        {
            return (treatZeroAsEmpty && value.Ticks == 0) ? string.Empty : value.ToString("yyyy/MM/dd HH:mm:ss");
        }
        /// <summary>把时间转换为完整的日期时间格式(yyyy/MM/dd HH:mm:ss.fff)</summary>
        public static string ToFullDateTimeString(this DateTime value)
        {
            return value.ToString("yyyy/MM/dd HH:mm:ss.fff");
        }
        #endregion

        #region 季度方法
        /// <summary>季度数组</summary>
        public static readonly string[] Quarters = { "一季度", "二季度", "三季度", "四季度" };
        /// <summary>根据月份获得季度(从零开始)</summary>
        public static int GetQuarter(int month)
        {
            var ceiling = Math.DivRem(month, 3, out var _);
            return ceiling - 1;
        }
        #endregion
    }
}
