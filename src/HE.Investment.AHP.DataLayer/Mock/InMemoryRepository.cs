using System.Collections.Concurrent;
using HE.InvestmentLoans.Common.Exceptions;

namespace HE.Investment.AHP.DataLayer.Mock;

public abstract class InMemoryRepository<TEntity> : IInMemoryRepository<TEntity>
{
    private static readonly IDictionary<string, TEntity> Entities = new ConcurrentDictionary<string, TEntity>();

    public Task<TEntity> GetById(string id, CancellationToken cancellationToken)
    {
        var item = Get(id);
        if (item != null)
        {
            return Task.FromResult(item);
        }

        throw new NotFoundException(nameof(TEntity), id);
    }

    public async Task<IList<TEntity>> GetAll(CancellationToken cancellationToken)
    {
        return await Task.FromResult(Entities.Values.ToList());
    }

    public Task<TEntity> Save(
        string id,
        TEntity entity,
        CancellationToken cancellationToken)
    {
        Save(id, entity);
        return Task.FromResult(entity);
    }

    private TEntity? Get(string id)
    {
        if (Entities.TryGetValue(id, out var entity))
        {
            return entity;
        }

        return default;
    }

    private void Save(string id, TEntity entity)
    {
        if (Entities.ContainsKey(id))
        {
            Entities.Remove(id);
        }

        Entities[id] = entity;
    }
}
