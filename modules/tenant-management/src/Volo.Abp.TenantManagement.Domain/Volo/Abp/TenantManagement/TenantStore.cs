﻿using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.TenantManagement
{
    //TODO: This class should use caching instead of querying everytime!

    public class TenantStore : ITenantStore, ITransientDependency
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IObjectMapper<AbpTenantManagementDomainModule> _objectMapper;
        private readonly ICurrentTenant _currentTenant;

        public TenantStore(
            ITenantRepository tenantRepository, 
            IObjectMapper<AbpTenantManagementDomainModule> objectMapper,
            ICurrentTenant currentTenant)
        {
            _tenantRepository = tenantRepository;
            _objectMapper = objectMapper;
            _currentTenant = currentTenant;
        }

        public async Task<TenantConfiguration> FindAsync(string name)
        {
            using (_currentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await FindByNameAsync(name);
                if (tenant == null)
                {
                    return null;
                }

                return _objectMapper.Map<Tenant, TenantConfiguration>(tenant);
            }
        }
        protected virtual async Task<Tenant> FindByNameAsync(string name)
        {
            return await _tenantRepository.FindByNameAsync(name);
        }

        public async Task<TenantConfiguration> FindAsync(Guid id)
        {
            using (_currentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await FindByIdAsync(id);
                if (tenant == null)
                {
                    return null;
                }

                return _objectMapper.Map<Tenant, TenantConfiguration>(tenant);
            }
        }

        protected virtual async Task<Tenant> FindByIdAsync(Guid id)
        {
            return await _tenantRepository.FindAsync(id);
        }
        public TenantConfiguration Find(string name)
        {
            using (_currentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = FindByName(name);
                if (tenant == null)
                {
                    return null;
                }

                return _objectMapper.Map<Tenant, TenantConfiguration>(tenant);
            }
        }

        public TenantConfiguration Find(Guid id)
        {
            using (_currentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = FindById(id);
                if (tenant == null)
                {
                    return null;
                }

                return _objectMapper.Map<Tenant, TenantConfiguration>(tenant);
            }
        }

        protected virtual Tenant FindByName(string name)
        {
            return _tenantRepository.FindByName(name);
        }

        protected virtual Tenant FindById(Guid id)
        {
            return _tenantRepository.Find(id);
        }
    }
}
