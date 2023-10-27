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

    public IEnumerable<HomeTypeBasicDetailsEntity> HomeTypes => _homeTypes;

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
                    new(nameof(HomeTypeName), "Enter a different name. Home types cannot have the same name")
                }));
        }
    }

    public HomeTypeName DuplicateName(HomeTypeId homeTypeId)
    {
        var homeType = _homeTypes.SingleOrDefault(x => x.Id == homeTypeId);
        if (homeType == null)
        {
            throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);
        }

        var suffixIndex = 1;
        var duplicatedName = homeType.Name ?? new HomeTypeName("Duplicate");

        while (_homeTypes.Any(x => x.Name == duplicatedName))
        {
            duplicatedName = duplicatedName.Duplicate(suffixIndex++);
        }

        return duplicatedName;
    }
}
