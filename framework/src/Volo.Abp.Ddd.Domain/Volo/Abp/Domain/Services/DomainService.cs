using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Services
{
    public abstract class DomainService : IDomainService
    {
        public IServiceProvider ServiceProvider { get; set; }
        protected readonly object ServiceProviderLock = new object();
        /// <summary>获得服务实例</summary>
        protected T GetRequiredService<T>()
        {
            return this.ServiceProvider.GetRequiredService<T>();
        }
        /// <summary>获得实体仓储</summary>
        protected IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IEntity
        {
            return GetRequiredService<IRepository<TEntity>>();
        }
        /// <summary>获得实体仓储</summary>
        protected IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
        {
            return GetRequiredService<IRepository<TEntity, TKey>>();
        }
        protected TService LazyGetRequiredService<TService>(ref TService reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = ServiceProvider.GetRequiredService<TService>();
                    }
                }
            }

            return reference;
        }

        public IClock Clock => LazyGetRequiredService(ref _clock);
        private IClock _clock;

        public IGuidGenerator GuidGenerator { get; set; }

        public ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);
        private ILoggerFactory _loggerFactory;

        public ICurrentTenant CurrentTenant => LazyGetRequiredService(ref _currentTenant);
        private ICurrentTenant _currentTenant;

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

        /// <summary>内存缓存</summary>
        private Microsoft.Extensions.Caching.Memory.IMemoryCache _memoryCache;
        /// <summary>内存缓存</summary>
        public Microsoft.Extensions.Caching.Memory.IMemoryCache MemoryCache => LazyGetRequiredService(ref _memoryCache);

        /// <summary>对象映射</summary>
        private IObjectMapper _objectMapper;
        /// <summary>内存缓存</summary>
        protected IObjectMapper ObjectMapper => LazyGetRequiredService(ref _objectMapper);
        /// <summary>对象映射</summary>
        protected T MapTo<T>(object source)
            where T : class
        {
            return ObjectMapper.Map(source.GetType(), typeof(T), source).As<T>();
        }
        /// <summary>对象映射</summary>
        protected void MapTo(object source, object destination)
        {
            ObjectMapper.Map(source.GetType(), destination.GetType(), source, destination);
        }
        /// <summary>工作单元管理器</summary>
        private IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>工作单元管理器</summary>
        protected IUnitOfWorkManager UnitOfWorkManager => LazyGetRequiredService(ref _unitOfWorkManager);
        /// <summary>当前工作单元</summary>
        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

        protected DomainService()
        {
            GuidGenerator = SimpleGuidGenerator.Instance;
        }
    }
}