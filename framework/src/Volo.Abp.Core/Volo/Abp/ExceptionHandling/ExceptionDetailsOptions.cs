using System;
using System.Collections.Generic;

namespace Volo.Abp.ExceptionHandling
{
    public class ExceptionDetailsOptions
    {
        public Dictionary<Type, IExceptionDetailsConvertContributor> ConvertContributors { get; }

        public ExceptionDetailsOptions()
        {
            ConvertContributors = new Dictionary<Type, IExceptionDetailsConvertContributor>();
            ConvertContributors.Add(typeof(Exception), EmptyExceptionDetailsConvertContributor.Instance);
            ConvertContributors.Add(typeof(AggregateException), EmptyExceptionDetailsConvertContributor.Instance);
        }

        public void Add(IExceptionDetailsConvertContributor contributor)
        {
            foreach (var type in contributor.CanConvertTo())
            {
                ConvertContributors[type] = contributor;
            }
        }
    }
}
