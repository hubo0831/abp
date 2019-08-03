﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Configuration;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.Internal
{
    internal static class InternalServiceCollectionExtensions
    {
        internal static void AddCoreServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging();
            services.AddLocalization();
        }

        internal static void AddCoreAbpServices(this IServiceCollection services,
            IAbpApplication abpApplication,
            AbpApplicationCreationOptions applicationCreationOptions)
        {
            var moduleLoader = new ModuleLoader();
            var assemblyFinder = new AssemblyFinder(abpApplication);
            var typeFinder = new TypeFinder(assemblyFinder);

            IConfigurationRoot configuration;
            if (applicationCreationOptions.UseDefaultConfiguration)
            {
                configuration = services.GetSingletonInstance<IConfiguration>().As<IConfigurationRoot>();
            }
            else
            {
                configuration = ConfigurationHelper.BuildConfiguration(applicationCreationOptions.Configuration);
            }
            services.TryAddSingleton<IConfigurationAccessor>(
                new DefaultConfigurationAccessor(configuration)
            );

            services.TryAddSingleton<IModuleLoader>(moduleLoader);
            services.TryAddSingleton<IAssemblyFinder>(assemblyFinder);
            services.TryAddSingleton<ITypeFinder>(typeFinder);

            services.AddAssemblyOf<IAbpApplication>();

            services.Configure<ModuleLifecycleOptions>(options =>
            {
                options.Contributors.Add<OnPreApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnPostApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnApplicationShutdownModuleLifecycleContributor>();
            });
        }
    }
}