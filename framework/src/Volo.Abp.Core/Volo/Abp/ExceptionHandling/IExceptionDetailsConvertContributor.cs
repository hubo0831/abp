using System;
using System.Collections.Generic;

namespace Volo.Abp.ExceptionHandling
{
    public interface IExceptionDetailsConvertContributor
    {
        IEnumerable<Type> CanConvertTo();
        string ConvertTo(Exception ex);
    }
}