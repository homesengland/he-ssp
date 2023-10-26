using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class HomeTypesEntity
{
    private readonly IList<HomeTypeBasicDetailsEntity> _homeTypes;

    public HomeTypesEntity(IEnumerable<HomeTypeBasicDetailsEntity> homeTypes)
    {
        _homeTypes = homeTypes.ToList();
    }

    public void ValidateNameUniqueness(HomeTypeId? homeTypeId, string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        if (_homeTypes.Where(x => x.Id != homeTypeId).Any(x => x.Name?.Value == name))
        {
            throw new DomainValidationException(
                new OperationResult().AddValidationErrors(new List<ErrorItem>
                {
                    new(nameof(HomeTypeName), "Enter a different name. Home types cannot have the same name"),
                }));
        }
    }
}
