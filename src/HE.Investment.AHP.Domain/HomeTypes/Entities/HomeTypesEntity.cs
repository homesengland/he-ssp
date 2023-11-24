using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Exceptions;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class HomeTypesEntity
{
    private readonly IList<HomeTypeEntity> _homeTypes;

    private readonly IList<HomeTypeEntity> _toRemove = new List<HomeTypeEntity>();

    private readonly ApplicationBasicInfo _application;

    private readonly ModificationTracker _statusModificationTracker = new();

    public HomeTypesEntity(ApplicationBasicInfo application, IEnumerable<HomeTypeEntity> homeTypes, SectionStatus status)
    {
        _application = application;
        _homeTypes = homeTypes.ToList();
        Status = status;
    }

    public ApplicationId ApplicationId => _application.Id;

    public ApplicationName ApplicationName => _application.Name;

    public IEnumerable<IHomeTypeEntity> HomeTypes => _homeTypes;

    public IEnumerable<IHomeTypeEntity> ToRemove => _toRemove;

    public SectionStatus Status { get; private set; }

    public bool IsStatusChanged => _statusModificationTracker.IsModified;

    public IHomeTypeEntity CreateHomeType(string? name, HousingType housingType)
    {
        var homeType = new HomeTypeEntity(_application, ValidateNameUniqueness(name), housingType);
        _homeTypes.Add(homeType);

        return homeType;
    }

    public void Remove(HomeTypeId homeTypeId)
    {
        var homeType = GetEntityById(homeTypeId);
        if (homeType.IsUsedInDeliveryPhase)
        {
            throw new DomainValidationException(
                new OperationResult().AddValidationErrors(new List<ErrorItem>
                {
                    new($"HomeType-{homeTypeId}", "Home Type cannot be removed because it is used in Delivery Phase"),
                }));
        }

        _toRemove.Add(homeType);
        _homeTypes.Remove(homeType);
    }

    public void ChangeName(IHomeTypeEntity homeTypeEntity, string? name)
    {
        var entity = _homeTypes.SingleOrDefault(x => x == homeTypeEntity) ?? throw new ArgumentException(
            $"Given {nameof(HomeTypeEntity)} does not belong to current {nameof(HomeTypesEntity)}",
            nameof(homeTypeEntity));

        entity.ChangeName(ValidateNameUniqueness(name, entity));
    }

    public void CompleteSection(FinishHomeTypesAnswer finishAnswer)
    {
        if (finishAnswer == FinishHomeTypesAnswer.Yes)
        {
            if (!_homeTypes.Any())
            {
                throw new DomainValidationException(
                    new OperationResult().AddValidationErrors(new List<ErrorItem>
                    {
                        new("HomeTypes", "Home Types cannot be completed because at least one Home Type needs to be added."),
                    }));
            }

            var notCompletedHomeTypes = _homeTypes.Where(x => !x.IsCompleted()).ToList();
            if (notCompletedHomeTypes.Any())
            {
                throw new DomainValidationException(new OperationResult().AddValidationErrors(
                    notCompletedHomeTypes.Select(x => new ErrorItem($"HomeType-{x.Id}", $"Complete {x.Name.Value} to save and continue")).ToList()));
            }

            Status = _statusModificationTracker.Change(Status, SectionStatus.Completed);
        }
        else
        {
            Status = _statusModificationTracker.Change(Status, SectionStatus.InProgress);
        }
    }

    public void MarkAsInProgress()
    {
        Status = _statusModificationTracker.Change(Status, SectionStatus.InProgress);
    }

    public IHomeTypeEntity Duplicate(HomeTypeId homeTypeId)
    {
        var homeType = GetEntityById(homeTypeId);
        var newName = GenerateUniqueNameDuplicate(homeType);

        return homeType.Duplicate(newName);
    }

    public HomeTypeEntity GetEntityById(HomeTypeId homeTypeId) => _homeTypes.SingleOrDefault(x => x.Id == homeTypeId)
                                                                   ?? throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);

    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    private string? ValidateNameUniqueness(string? name, HomeTypeEntity? entity = null)
    {
        if ((entity == null && _homeTypes.Any(x => x.Name.Value == name))
            || (entity != null && _homeTypes.Except(new[] { entity }).Any(x => x.Name.Value == name)))
        {
            throw new DomainValidationException(
                new OperationResult().AddValidationErrors(new List<ErrorItem>
                {
                    new(nameof(HomeTypeName), "Enter a different name. Home types cannot have the same name"),
                }));
        }

        return name;
    }

    private HomeTypeName GenerateUniqueNameDuplicate(IHomeTypeEntity homeType)
    {
        var suffixIndex = 1;
        var duplicatedName = homeType.Name;

        while (_homeTypes.Any(x => x.Name == duplicatedName))
        {
            duplicatedName = duplicatedName.Duplicate(suffixIndex++);
        }

        return duplicatedName;
    }
}
