using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class HomeTypesEntity
{
    private readonly IList<HomeTypeEntity> _homeTypes;

    private readonly ApplicationBasicInfo _application;

    public HomeTypesEntity(ApplicationBasicInfo application, IEnumerable<HomeTypeEntity> homeTypes)
    {
        _application = application;
        _homeTypes = homeTypes.ToList();
    }

    public IEnumerable<IHomeTypeEntity> HomeTypes => _homeTypes;

    public IHomeTypeEntity GetOrCreateNewHomeType(HomeTypeId? homeTypeId = null)
    {
        if (homeTypeId.IsProvided())
        {
            return GetEntityById(homeTypeId!);
        }

        // TODO: remove creating segments when integration with CRM is introduced
        var homeType = new HomeTypeEntity(
            _application,
            segments: new IHomeTypeSegmentEntity[]
            {
                new HomeInformationSegmentEntity(),
                new DisabledPeopleHomeTypeDetailsSegmentEntity(),
                new OlderPeopleHomeTypeDetailsSegmentEntity(),
                new DesignPlansSegmentEntity(_application),
            });
        _homeTypes.Add(homeType);

        return homeType;
    }

    public void ChangeName(IHomeTypeEntity homeTypeEntity, string? name)
    {
        var entity = _homeTypes.SingleOrDefault(x => x == homeTypeEntity) ?? throw new ArgumentException(
            $"Given {nameof(HomeTypeEntity)} does not belong to current {nameof(HomeTypesEntity)}",
            nameof(homeTypeEntity));

        if (!string.IsNullOrEmpty(name) && _homeTypes.Except(new[] { entity }).Any(x => x.Name?.Value == name))
        {
            throw new DomainValidationException(
                new OperationResult().AddValidationErrors(new List<ErrorItem>
                {
                    new(nameof(HomeTypeName), "Enter a different name. Home types cannot have the same name"),
                }));
        }

        entity.ChangeName(name);
    }

    public IHomeTypeEntity Duplicate(HomeTypeId homeTypeId)
    {
        var homeType = GetEntityById(homeTypeId);
        var newName = GenerateUniqueNameDuplicate(homeType);

        return homeType.Duplicate(newName);
    }

    private HomeTypeEntity GetEntityById(HomeTypeId homeTypeId) => _homeTypes.SingleOrDefault(x => x.Id == homeTypeId)
                                                                   ?? throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);

    private HomeTypeName GenerateUniqueNameDuplicate(IHomeTypeEntity homeType)
    {
        var suffixIndex = 1;
        var duplicatedName = homeType.Name ?? new HomeTypeName("Duplicate");

        while (_homeTypes.Any(x => x.Name == duplicatedName))
        {
            duplicatedName = duplicatedName.Duplicate(suffixIndex++);
        }

        return duplicatedName;
    }
}
