using System.Collections.Concurrent;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;

namespace HE.Investment.AHP.Domain.Scheme;

public class SchemeRepository : ISchemeRepository
{
    private static readonly IDictionary<string, SchemeEntity> Schemes = new ConcurrentDictionary<string, SchemeEntity>();

    public Task<SchemeEntity> GetById(SchemeId schemeId, CancellationToken cancellationToken)
    {
        var item = Get(schemeId);
        if (item != null)
        {
            return Task.FromResult(item);
        }

        throw new NotFoundException(nameof(SchemeEntity), schemeId);
    }

    public async Task<IList<SchemeEntity>> GetAll(CancellationToken cancellationToken)
    {
        return await Task.FromResult(Schemes.Values.ToList());
    }

    public Task<SchemeEntity> Save(
        SchemeEntity scheme,
        CancellationToken cancellationToken)
    {
        Save(scheme);
        return Task.FromResult(scheme);
    }

    private SchemeEntity? Get(SchemeId schemeId)
    {
        if (Schemes.TryGetValue(schemeId.ToString(), out var schemeEntity))
        {
            return schemeEntity;
        }

        return null;
    }

    private void Save(SchemeEntity entity)
    {
        if (Schemes.ContainsKey(entity.Id!.Value))
        {
            Schemes.Remove(entity.Id!.Value);
        }

        Schemes[entity.Id!.Value] = entity;
    }
}
