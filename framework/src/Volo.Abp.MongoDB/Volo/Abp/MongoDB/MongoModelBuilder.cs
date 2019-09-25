using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.MongoDB
{
    public class MongoModelBuilder : IMongoModelBuilder
    {
        private readonly Dictionary<Type, object> _entityModelBuilders;

        private static readonly object SyncObj = new object();

        public MongoModelBuilder()
        {
            _entityModelBuilders = new Dictionary<Type, object>();
        }

        public MongoDbContextModel Build()
        {
            lock (SyncObj)
            {
                var entityModels = _entityModelBuilders
                    .Select(x => x.Value)
                    .Cast<IMongoEntityModel>()
                    .ToImmutableDictionary(x => x.EntityType, x => x);

                var baseClasses = new List<Type>();

                foreach (var entityModel in entityModels.Values)
                {
                    var map = entityModel.As<IHasBsonClassMap>().GetMap();
                    if (!BsonClassMap.IsClassMapRegistered(map.ClassType))
                    {
                        BsonClassMap.RegisterClassMap(map);
                    }

                    baseClasses.AddRange(entityModel.EntityType.GetBaseClasses(includeObject: false));
                }

                baseClasses = baseClasses.Distinct().ToList();

                foreach (var baseClass in baseClasses)
                {
                    if (!BsonClassMap.IsClassMapRegistered(baseClass))
                    {
                        var map = new BsonClassMap(baseClass);
                        map.ConfigureAbpConventions();
                        BsonClassMap.RegisterClassMap(map);
                    }
                }

                return new MongoDbContextModel(entityModels);
            }
        }

        public virtual void Entity<TEntity>(Action<IMongoEntityModelBuilder<TEntity>> buildAction = null, Type baseEntityType = null)
        {
            var baseClassMap = GetBsonClassMap(typeof(TEntity), baseEntityType);

            var model = (IMongoEntityModelBuilder<TEntity>)_entityModelBuilders.GetOrAdd(
                typeof(TEntity),
                () => new MongoEntityModelBuilder<TEntity>(baseClassMap)
            );

            buildAction?.Invoke(model);
        }

        public virtual void Entity(Type entityType, Action<IMongoEntityModelBuilder> buildAction = null, Type baseEntityType = null)
        {
            Check.NotNull(entityType, nameof(entityType));

            var baseClassMap = GetBsonClassMap(entityType, baseEntityType);

            var model = (IMongoEntityModelBuilder)_entityModelBuilders.GetOrAdd(
                entityType,
                () => (IMongoEntityModelBuilder)Activator.CreateInstance(
                    typeof(MongoEntityModelBuilder<>).MakeGenericType(entityType), baseClassMap
                )
            );

            buildAction?.Invoke(model);
        }
        public virtual BsonClassMap GetBsonClassMap(Type entityType, Type baseEntityType = null)
        {
            if (baseEntityType == null) return null;
            if (!baseEntityType.IsAssignableFrom(entityType))
            {
                throw new AbpException($"{baseEntityType.FullName}必须是{entityType.FullName}的基类！");
            }
            if (_entityModelBuilders.TryGetValue(baseEntityType, out var entityModelBuilder))
            {
                return entityModelBuilder.As<IMongoEntityModelBuilder>().BsonMap;
            }
            return null;
        }

        public virtual IReadOnlyList<IMongoEntityModel> GetEntities()
        {
            return _entityModelBuilders.Values.Cast<IMongoEntityModel>().ToImmutableList();
        }
    }
}