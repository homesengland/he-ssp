using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.DesignPlans)]
public class DesignPlansSegmentEntity : IHomeTypeSegmentEntity
{
    private const int AllowedNumberOfDesignFiles = 10;

    private readonly ApplicationBasicInfo _application;

    private readonly List<HappiDesignPrincipleType> _designPrinciples;

    private readonly IList<UploadedFile> _uploadedFiles;

    private readonly IList<UploadedFile> _filesToRemove = new List<UploadedFile>();

    private readonly IList<DesignPlanFileEntity> _filesToUpload = new List<DesignPlanFileEntity>();

    private readonly ModificationTracker _modificationTracker;

    public DesignPlansSegmentEntity(
        ApplicationBasicInfo application,
        IEnumerable<HappiDesignPrincipleType>? designPrinciples = null,
        MoreInformation? moreInformation = null,
        IEnumerable<UploadedFile>? uploadedFiles = null)
    {
        _modificationTracker = new ModificationTracker(() => SegmentModified?.Invoke());
        _application = application;
        _designPrinciples = designPrinciples?.OrderBy(x => x).ToList() ?? new List<HappiDesignPrincipleType>();
        MoreInformation = moreInformation;
        _uploadedFiles = uploadedFiles?.ToList() ?? new List<UploadedFile>();
    }

    public event EntityModifiedEventHandler SegmentModified;

    public bool IsModified => _modificationTracker.IsModified;

    public IReadOnlyCollection<HappiDesignPrincipleType> DesignPrinciples => _designPrinciples;

    public MoreInformation? MoreInformation { get; private set; }

    public bool CanRemoveDesignFiles => _application.Status != ApplicationStatus.ApplicationSubmitted;

    public IEnumerable<UploadedFile> UploadedFiles => _uploadedFiles;

    public void ChangeDesignPrinciples(IEnumerable<HappiDesignPrincipleType> designPrinciples)
    {
        var uniquePrinciples = designPrinciples.Distinct().OrderBy(x => x).ToList();
        if (uniquePrinciples.Contains(HappiDesignPrincipleType.NoneOfThese) && uniquePrinciples.Count > 1)
        {
            OperationResult.New()
                .AddValidationError(
                    nameof(DesignPrinciples),
                    ValidationErrorMessage.ExclusiveOptionSelected("design principle", HappiDesignPrincipleType.NoneOfThese.GetDescription()))
                .CheckErrors();
        }

        if (!_designPrinciples.SequenceEqual(uniquePrinciples))
        {
            _designPrinciples.Clear();
            _designPrinciples.AddRange(uniquePrinciples);
            _modificationTracker.MarkAsModified();
        }
    }

    public void ChangeMoreInformation(string? moreInformation)
    {
        var newValue = string.IsNullOrEmpty(moreInformation) ? null : new MoreInformation(moreInformation);
        MoreInformation = _modificationTracker.Change(MoreInformation, newValue);
    }

    public void AddFilesToUpload(IReadOnlyCollection<DesignPlanFileEntity> files)
    {
        if (_uploadedFiles.Count + _filesToUpload.Count + files.Count - _filesToRemove.Count > AllowedNumberOfDesignFiles)
        {
            OperationResult.New().AddValidationError("File", GenericValidationError.FileCountLimit(AllowedNumberOfDesignFiles)).CheckErrors();
        }

        foreach (var file in files)
        {
            if (_uploadedFiles.Any(x => x.Name == file.Name) || _filesToUpload.Any(x => x.Name == file.Name))
            {
                OperationResult.New().AddValidationError("File", GenericValidationError.FileUniqueName).CheckErrors();
            }

            _filesToUpload.Add(file);
        }

        _modificationTracker.MarkAsModified();
    }

    public void MarkFileToRemove(FileId fileId)
    {
        var fileToRemove = _uploadedFiles.FirstOrDefault(x => x.Id == fileId) ?? throw new NotFoundException(nameof(DesignPlanFileEntity), fileId);

        if (_application.Status == ApplicationStatus.ApplicationSubmitted)
        {
            new OperationResult().AddValidationError("File", "File cannot be removed because Application is already Submitted.").CheckErrors();
        }

        _filesToRemove.Add(fileToRemove);
        _modificationTracker.MarkAsModified();
    }

    public async Task SaveFileChanges(IHomeTypeEntity homeType, IAhpFileService<DesignFileParams> designFileService, CancellationToken cancellationToken)
    {
        for (var i = _filesToUpload.Count - 1; i >= 0; i--)
        {
            var uploadedFile = await _filesToUpload[i].Upload(homeType, designFileService, cancellationToken);
            _uploadedFiles.Add(uploadedFile);
            _filesToUpload.RemoveAt(i);
        }

        for (var i = _filesToRemove.Count - 1; i >= 0; i--)
        {
            await designFileService.RemoveFile(_filesToRemove[i].Id, new DesignFileParams(homeType.Application.Id, homeType.Id), cancellationToken);
            _uploadedFiles.Remove(_filesToRemove[i]);
            _filesToRemove.RemoveAt(i);
        }
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new DesignPlansSegmentEntity(_application, DesignPrinciples, MoreInformation, new List<UploadedFile>());
    }

    public bool IsRequired(HousingType housingType)
    {
        return housingType is HousingType.HomesForOlderPeople or HousingType.HomesForDisabledAndVulnerablePeople;
    }

    public bool IsCompleted(HousingType housingType, Tenure tenure)
    {
        return DesignPrinciples.Any();
    }

    public void HousingTypeChanged(HousingType sourceHousingType, HousingType targetHousingType)
    {
        if (targetHousingType is HousingType.Undefined or HousingType.General)
        {
            ChangeDesignPrinciples(Enumerable.Empty<HappiDesignPrincipleType>());
            ChangeMoreInformation(null);
        }
    }
}
