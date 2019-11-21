using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.MongoDB;

namespace Volo.Abp.MongoDB.DependencyInjection
{
    public class MongoDbRepositoryRegistrar : RepositoryRegistrarBase<AbpMongoDbContextRegistrationOptions>
    {
        public MongoDbRepositoryRegistrar(AbpMongoDbContextRegistrationOptions options)
            : base(options)
        {

        }
        public override void AddRepositories()
        {
            base.AddRepositories();
            if (Options.RegisterMongoDbRepositories)
            {
                RegisterMongoDbRepositories();
            }
        }

        protected virtual void RegisterMongoDbRepositories()
        {
            var repositoryInterfaceType = typeof(IMongoDbRepository<>);
            var repositoryInterfaceWithPkType = typeof(IMongoDbRepository<,>);
            foreach (var entityType in GetEntityTypes(Options.OriginalDbContextType))
            {
                if (Options.CustomRepositories.ContainsKey(entityType))
                {
                    continue;
                }
                var repositoryImplementationType = GetDefaultRepositoryImplementationType(entityType);
                Options.Services.AddRepository(entityType, repositoryInterfaceType, repositoryImplementationType);
                Options.Services.AddRepositoryWithPk(entityType, repositoryInterfaceWithPkType, repositoryImplementationType);
            }
        }

        protected override IEnumerable<Type> GetEntityTypes(Type dbContextType)
        {
            return MongoDbContextHelper.GetEntityTypes(dbContextType);
        }

        protected override Type GetRepositoryType(Type dbContextType, Type entityType)
        {
            return typeof(MongoDbRepository<,>).MakeGenericType(dbContextType, entityType);
        }

        protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
        {
            return typeof(MongoDbRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);
        }
    }
}