using System;

namespace Volo.Abp.ExceptionHandling
{
    public interface IExceptionDetailsConverter
    {
        string ConvertTo(Exception ex);
    }
}