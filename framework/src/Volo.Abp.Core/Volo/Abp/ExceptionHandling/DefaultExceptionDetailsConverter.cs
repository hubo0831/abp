using System;
using System.Collections.Generic;

using Microsoft.Extensions.Options;

using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ExceptionHandling
{
    public class DefaultExceptionDetailsConverter : IExceptionDetailsConverter, ISingletonDependency
    {
        public DefaultExceptionDetailsConverter(IOptions<ExceptionDetailsOptions> options)
        {
            this.Options = options.Value;
        }
        private ExceptionDetailsOptions Options { get; }

        public string ConvertTo(Exception ex)
        {
            var exType = ex.GetType();
            var contributors = Options.ConvertContributors;
            if (!contributors.TryGetValue(exType, out var contributor))
            {
                var exTypes = new List<Type> { exType };
                exType = exType.BaseType;
                while (true)
                {
                    if (contributors.TryGetValue(exType, out contributor))
                    {
                        foreach (var item in exTypes)
                        {
                            if (contributors.ContainsKey(item)) continue;
                            contributors.Add(item, contributor);
                        }
                        break;
                    }
                    exTypes.Add(exType);
                    exType = exType.BaseType;
                }
            }
            return contributor.ConvertTo(ex);
        }
    }
}