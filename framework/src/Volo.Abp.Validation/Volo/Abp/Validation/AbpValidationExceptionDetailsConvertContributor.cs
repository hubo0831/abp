using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.Validation
{
    public class AbpValidationExceptionDetailsConvertContributor : IExceptionDetailsConvertContributor
    {
        public IEnumerable<Type> CanConvertTo()
        {
            return new Type[] { typeof(AbpValidationException) };
        }
        public string ConvertTo(Exception ex)
        {
            var vex = ex.As<AbpValidationException>();
            var builder = new StringBuilder();
            foreach (var item in vex.ValidationErrors)
            {
                if (item.MemberNames.Count() > 0)
                {
                    builder.Append("[").Append(string.Join(",", item.MemberNames)).Append("]");
                }
                builder.Append(item.ErrorMessage);
            }
            return builder.ToString();
        }
    }
}