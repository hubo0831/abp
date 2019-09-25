using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MongoDB.Bson.Serialization;

namespace Volo.Abp.MongoDB
{
    public interface IMongoModelBuilder
    {
        void Entity<TEntity>(Action<IMongoEntityModelBuilder<TEntity>> buildAction = null, Type baseEntityType = null);

        void Entity([NotNull] Type entityType, Action<IMongoEntityModelBuilder> buildAction = null, Type baseEntityType = null);

        IReadOnlyList<IMongoEntityModel> GetEntities();
    }
}