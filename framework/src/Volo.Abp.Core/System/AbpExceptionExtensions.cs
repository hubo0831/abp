using System.Runtime.ExceptionServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Logging;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="Exception"/> class.
    /// </summary>
    public static class AbpExceptionExtensions
    {
        /// <summary>
        /// Uses <see cref="ExceptionDispatchInfo.Capture"/> method to re-throws exception
        /// while preserving stack trace.
        /// </summary>
        /// <param name="exception">Exception to be re-thrown</param>
        public static void ReThrow(this Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }

        /// <summary>
        /// Try to get a log level from the given <paramref name="exception"/>
        /// if it implements the <see cref="IHasLogLevel"/> interface.
        /// Otherwise, returns the <paramref name="defaultLevel"/>.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="defaultLevel"></param>
        /// <returns></returns>
        public static LogLevel GetLogLevel(this Exception exception, LogLevel defaultLevel = LogLevel.Error)
        {
            return (exception as IHasLogLevel)?.LogLevel ?? defaultLevel;
        }
        /// <summary>获得异常的完整信息</summary>
        public static string GetFullMessage(this Exception ex, IExceptionDetailsConverter converter = null)
        {
            var builder = new StringBuilder(512);
            BuildMessage(converter, builder, ex);
            return builder.ToString();
        }
        /// <summary>生成异常信息</summary>
        private static void BuildMessage(IExceptionDetailsConverter converter, StringBuilder builder, Exception ex)
        {
            builder.Append(ex.Message);
            if (converter != null)
            {
                var details = converter.ConvertTo(ex);
                builder.Append(details);
            }
            var innerException = ex.InnerException;
            if (innerException != null)
            {
                BuildMessage(converter, builder, innerException);
            }
            if (ex is AggregateException aex)
            {
                foreach (var innerException2 in aex.InnerExceptions)
                {
                    BuildMessage(converter, builder, innerException2);
                }
            }
        }
    }
}