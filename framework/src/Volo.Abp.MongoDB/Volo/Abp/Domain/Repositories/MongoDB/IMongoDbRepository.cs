using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public interface IMongoDbRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        IMongoDatabase Database { get; }

        IClientSessionHandle Session { get; }

        IMongoCollection<TEntity> Collection { get; }

        IMongoQueryable<TEntity> GetMongoQueryable();

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filter, FindOptions<TEntity, TEntity> options = null, CancellationToken cancellationToken = default);

        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter, FindOptions<TEntity, TEntity> options = null, CancellationToken cancellationToken = default);

        Task<List<TEntity>> FindAsync(FilterDefinition<TEntity> filter, FindOptions<TEntity, TEntity> options = null, CancellationToken cancellationToken = default);

        Task InsertAsync(IEnumerable<TEntity> entities, InsertManyOptions options = null, CancellationToken cancellationToken = default);

        Task DeleteAsync(IEnumerable<TEntity> documents, CancellationToken cancellationToken = default);

        Task FastDeleteAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
    }

    public interface IMongoDbRepository<TEntity, TKey> : IMongoDbRepository<TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {

    }
}
