using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Quartz;

using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace Volo.Abp.BackgroundJobs.Quartz
{
    /// <summary>Quartz任务基类</summary>
    public abstract class AbpQuartzJob : DomainService, IJob
    {
        /// <summary>执行任务</summary>
        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = this.ServiceProvider.CreateScope())
            {
                this.ServiceProvider = scope.ServiceProvider;
                using (var uow = this.UnitOfWorkManager.Begin(AbpUnitOfWorkOptions.Default))
                {
                    var tenantId = GetTenantId(context);
                    using (this.CurrentTenant.Change(tenantId))
                    {
                        try
                        {
                            await ExecuteInternal(context);
                            await uow.CompleteAsync();
                        }
                        catch (Exception ex)
                        {
                            var jex = HandleJobExecutionException(context, ex);
                            if (jex == null) throw;
                            throw jex;
                        }
                    }
                }
            }
        }
        /// <summary>获得租户Id</summary>
        protected virtual Guid? GetTenantId(IJobExecutionContext context)
        {
            return null;
        }
        /// <summary>处理任务执行异常</summary>
        protected virtual JobExecutionException HandleJobExecutionException(IJobExecutionContext context, Exception ex)
        {
            return null;
        }
        /// <summary>执行内部任务</summary>
        protected abstract Task ExecuteInternal(IJobExecutionContext context);
    }
}
