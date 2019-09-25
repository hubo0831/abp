using MongoDB.Bson.Serialization;
using System;
using System.Reflection;

namespace Volo.Abp.MongoDB
{
    public class MongoEntityModelBuilder<TEntity> :
        IMongoEntityModel,
        IHasBsonClassMap,
        IMongoEntityModelBuilder,
        IMongoEntityModelBuilder<TEntity>
    {
        public Type EntityType { get; }

        public string CollectionName { get; set; }

        BsonClassMap IMongoEntityModelBuilder.BsonMap => _bsonClassMap;
        BsonClassMap<TEntity> IMongoEntityModelBuilder<TEntity>.BsonMap => _bsonClassMap;

        private readonly BsonClassMap<TEntity> _bsonClassMap;

        public MongoEntityModelBuilder(BsonClassMap baseClassMap = null)
        {
            EntityType = typeof(TEntity);
            _bsonClassMap = new BsonClassMap<TEntity>();
            if (baseClassMap != null) SetBaseClassMap(baseClassMap);
            _bsonClassMap.ConfigureAbpConventions();
        }

        public BsonClassMap GetMap()
        {
            return _bsonClassMap;
        }
        private void SetBaseClassMap(BsonClassMap baseClassMap)
        {
            //bsonClassMap.BaseClassMap = baseClassMap;
            var fieldInfo = typeof(BsonClassMap).GetField("_baseClassMap", BindingFlags.Instance | BindingFlags.NonPublic);
            fieldInfo.SetValue(_bsonClassMap, baseClassMap);
        }
    }
}