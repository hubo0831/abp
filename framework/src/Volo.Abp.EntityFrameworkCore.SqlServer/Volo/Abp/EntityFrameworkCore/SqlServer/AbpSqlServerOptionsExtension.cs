﻿using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.SqlServer
{
    [Obsolete]
    public class AbpSqlServerOptionsExtension : SqlServerOptionsExtension
    {
        public AbpSqlServerOptionsExtension()
        {
            this.ReplaceServices = new ServiceCollection();
        }
        protected AbpSqlServerOptionsExtension(AbpSqlServerOptionsExtension copyFrom) : base(copyFrom)
        {
            this.ReplaceServices = copyFrom.ReplaceServices;
        }
        public IServiceCollection ReplaceServices { get; }
        public override void ApplyServices(IServiceCollection services)
        {
            foreach (var descriptor in this.ReplaceServices)
            {
                services.Add(descriptor);
            }
            this.ReplaceServices.Clear();
            base.ApplyServices(services);
        }
        protected override RelationalOptionsExtension Clone()
        {
            return new AbpSqlServerOptionsExtension(this);
        }
    }
}