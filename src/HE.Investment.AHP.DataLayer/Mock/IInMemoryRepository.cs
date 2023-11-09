namespace HE.Investment.AHP.DataLayer.Mock;

public interface IInMemoryRepository<TEntity>
{
    Task<TEntity> GetById(string id, CancellationToken cancellationToken);

    Task<IList<TEntity>> GetAll(CancellationToken cancellationToken);

    Task<TEntity> Save(
        string id,
        TEntity entity,
        CancellationToken cancellationToken);
}
