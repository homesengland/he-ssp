using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.Services;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using ApplicationStatus = HE.Investment.AHP.Domain.Common.ApplicationStatus;

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

    private readonly ModificationTracker _modificationTracker = new();

    public DesignPlansSegmentEntity(
        ApplicationBasicInfo application,
        IEnumerable<HappiDesignPrincipleType>? designPrinciples = null,
        MoreInformation? moreInformation = null,
        IEnumerable<UploadedFile>? uploadedFiles = null)
    {
        _application = application;
        _designPrinciples = designPrinciples?.OrderBy(x => x).ToList() ?? new List<HappiDesignPrincipleType>();
        MoreInformation = moreInformation;
        _uploadedFiles = uploadedFiles?.ToList() ?? new List<UploadedFile>();
    }

    public bool IsModified => _modificationTracker.IsModified;

    public IReadOnlyCollection<HappiDesignPrincipleType> DesignPrinciples => _designPrinciples;

    public MoreInformation? MoreInformation { get; private set; }

    public bool CanRemoveDesignFiles => _application.Status != ApplicationStatus.Submitted;

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
            _filesToUpload.Add(file);
        }

        _modificationTracker.MarkAsModified();
    }

    public void MarkFileToRemove(FileId fileId)
    {
        var fileToRemove = _uploadedFiles.FirstOrDefault(x => x.Id == fileId) ?? throw new NotFoundException(nameof(DesignPlanFileEntity), fileId);

        if (_application.Status == ApplicationStatus.Submitted)
        {
            new OperationResult().AddValidationError("File", "File cannot be removed because Application is already Submitted.").CheckErrors();
        }

        _filesToRemove.Add(fileToRemove);
        _modificationTracker.MarkAsModified();
    }

    public async Task SaveFileChanges(IHomeTypeEntity homeType, IDesignFileService designFileService, CancellationToken cancellationToken)
    {
        for (var i = _filesToUpload.Count - 1; i >= 0; i--)
        {
            var uploadedFile = await _filesToUpload[i].Upload(homeType, designFileService, cancellationToken);
            _uploadedFiles.Add(uploadedFile);
            _filesToUpload.RemoveAt(i);
        }

        for (var i = _filesToRemove.Count - 1; i >= 0; i--)
        {
            await designFileService.RemoveFile(homeType.Application.Id, homeType.Id, _filesToRemove[i].Id, cancellationToken);
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
        return housingType is HousingType.HomesForOlderPeople or HousingType.HomesForOlderPeople;
    }

    public bool IsCompleted()
    {
        return DesignPrinciples.Any();
    }
}
