using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class HomeTypesEntity
{
    private readonly SiteBasicInfo _site;

    private readonly List<HomeTypeEntity> _homeTypes;

    private readonly List<HomeTypeEntity> _toRemove = [];

    private readonly ModificationTracker _statusModificationTracker = new();

    public HomeTypesEntity(ApplicationBasicInfo application, SiteBasicInfo site, IEnumerable<HomeTypeEntity> homeTypes, SectionStatus status)
    {
        _site = site;
        Application = application;
        _homeTypes = homeTypes.ToList();
        Status = status;
    }

    public ApplicationBasicInfo Application { get; }

    public IEnumerable<IHomeTypeEntity> HomeTypes => _homeTypes;

    public IEnumerable<IHomeTypeEntity> ToRemove => _toRemove;

    public SectionStatus Status { get; private set; }

    public bool IsStatusChanged => _statusModificationTracker.IsModified;

    public IHomeTypeEntity CreateHomeType(string? name, HousingType housingType)
    {
        var homeType = new HomeTypeEntity(Application, _site, ValidateNameUniqueness(name), housingType, SectionStatus.InProgress);
        _homeTypes.Add(homeType);

        return homeType;
    }

    public IHomeTypeEntity? PopRemovedHomeType() => _toRemove.PopItem();

    public void Remove(HomeTypeId homeTypeId, RemoveHomeTypeAnswer removeAnswer, IHomeTypeConsumer homeTypeConsumer)
    {
        if (removeAnswer == RemoveHomeTypeAnswer.Undefined)
        {
            OperationResult.New().AddValidationError(nameof(RemoveHomeTypeAnswer), "Select whether you want to remove this home type").CheckErrors();
        }

        if (removeAnswer == RemoveHomeTypeAnswer.Yes)
        {
            var homeType = GetEntityById(homeTypeId);
            if (homeTypeConsumer.IsHomeTypeUsed(homeTypeId))
            {
                OperationResult.New()
                    .AddValidationError($"HomeType-{homeTypeId}", $"Home Type cannot be removed because it is used in {homeTypeConsumer.HomeTypeConsumerName}")
                    .CheckErrors();
            }

            _toRemove.Add(homeType);
            _homeTypes.Remove(homeType);
        }
    }

    public void ChangeName(IHomeTypeEntity homeTypeEntity, string? name)
    {
        var entity = _homeTypes.SingleOrDefault(x => x == homeTypeEntity) ?? throw new ArgumentException(
            $"Given {nameof(HomeTypeEntity)} does not belong to current {nameof(HomeTypesEntity)}",
            nameof(homeTypeEntity));

        entity.ChangeName(ValidateNameUniqueness(name, entity));
    }

    public void CompleteSection(FinishHomeTypesAnswer finishAnswer, int? expectedNumberOfHomes)
    {
        if (finishAnswer == FinishHomeTypesAnswer.Undefined)
        {
            OperationResult.ThrowValidationError(nameof(finishAnswer), ValidationErrorMessage.NoCheckAnswers);
        }

        if (finishAnswer == FinishHomeTypesAnswer.Yes)
        {
            if (_homeTypes.Count == 0)
            {
                throw new DomainValidationException(
                    new OperationResult().AddValidationErrors(
                    [
                        new("HomeTypes", "Home Types cannot be completed because at least one Home Type needs to be added"),
                    ]));
            }

            var notCompletedHomeTypes = _homeTypes.Where(x => x.Status != SectionStatus.Completed).ToList();
            if (notCompletedHomeTypes.Count != 0)
            {
                throw new DomainValidationException(new OperationResult().AddValidationErrors(
                    notCompletedHomeTypes.Select(x => new ErrorItem($"HomeType-{x.Id}", $"Complete {x.Name.Value} to save and continue")).ToList()));
            }

            if (expectedNumberOfHomes.HasValue && _homeTypes.Sum(x => x.HomeInformation.NumberOfHomes?.Value ?? 0) < expectedNumberOfHomes)
            {
                OperationResult.New().AddValidationError("HomeTypes", "You have not assigned all of the homes you are delivering to a home type").CheckErrors();
            }

            if (expectedNumberOfHomes.HasValue && _homeTypes.Sum(x => x.HomeInformation.NumberOfHomes?.Value ?? 0) > expectedNumberOfHomes)
            {
                OperationResult.New().AddValidationError("HomeTypes", "Remove homes to match the numbers in your scheme").CheckErrors();
            }

            Status = _statusModificationTracker.Change(Status, SectionStatus.Completed);
        }
        else
        {
            MarkAsInProgress();
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
        if ((entity == null && _homeTypes.Exists(x => x.Name.Value == name))
            || (entity != null && _homeTypes.Except([entity]).Any(x => x.Name.Value == name)))
        {
            throw new DomainValidationException(
                new OperationResult().AddValidationErrors(
                [
                    new(nameof(HomeTypeName), "Enter a different name. Home types cannot have the same name"),
                ]));
        }

        return name;
    }

    private HomeTypeName GenerateUniqueNameDuplicate(IHomeTypeEntity homeType)
    {
        var suffixIndex = 1;
        var duplicatedName = homeType.Name;

        while (_homeTypes.Exists(x => x.Name == duplicatedName))
        {
            duplicatedName = duplicatedName.Duplicate(suffixIndex++);
        }

        return duplicatedName;
    }
}
