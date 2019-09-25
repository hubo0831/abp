using System;
using System.Collections.Generic;

namespace Volo.Abp.ExceptionHandling
{
    public class EmptyExceptionDetailsConvertContributor: IExceptionDetailsConvertContributor
    {
        private EmptyExceptionDetailsConvertContributor() { }
        public static EmptyExceptionDetailsConvertContributor Instance => new EmptyExceptionDetailsConvertContributor();
        public IEnumerable<Type> CanConvertTo()
        {
            return Array.Empty<Type>();
        }
        public string ConvertTo(Exception ex)
        {
            return string.Empty;
        }
    }
}