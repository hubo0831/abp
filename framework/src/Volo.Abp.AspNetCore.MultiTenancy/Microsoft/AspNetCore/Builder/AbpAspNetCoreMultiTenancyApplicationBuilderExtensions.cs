using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.MultiTenancy;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpAspNetCoreMultiTenancyApplicationBuilderExtensions
    {
        /// <summary>This key is from MvcApplicationBuilderExtensions.EndpointRoutingRegisteredKey</summary>
        private const string EndpointRouteBuilder = "__EndpointRouteBuilder";

        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
        {
            if (IsUseEndpointRouting(app))
            {
                app.UseRouting();
            }
            return app.UseMiddleware<MultiTenancyMiddleware>();
        }
        private static bool IsUseEndpointRouting(IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<AbpTenantResolveOptions>>().Value;
            if (!options.TenantResolvers.Any(e => e is RouteTenantResolveContributor))
            {
                return false;
            }
            if (!app.ApplicationServices.GetRequiredService<IOptions<MvcOptions>>().Value.EnableEndpointRouting)
            {
                return false;
            }
            if (app.Properties.TryGetValue(EndpointRouteBuilder, out _))
            {
                return false;
            }
            return true;
        }
    }
}
