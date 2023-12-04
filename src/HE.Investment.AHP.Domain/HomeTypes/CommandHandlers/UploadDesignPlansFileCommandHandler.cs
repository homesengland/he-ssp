using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;
using UploadedFileContract = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class UploadDesignPlansFileCommandHandler : IRequestHandler<UploadDesignPlansFileCommand, OperationResult<UploadedFileContract?>>
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    private readonly IAhpDocumentSettings _documentSettings;

    public UploadDesignPlansFileCommandHandler(IHomeTypeRepository homeTypeRepository, IAhpDocumentSettings documentSettings)
    {
        _homeTypeRepository = homeTypeRepository;
        _documentSettings = documentSettings;
    }

    public async Task<OperationResult<UploadedFileContract?>> Handle(UploadDesignPlansFileCommand request, CancellationToken cancellationToken)
    {
        var homeType = await _homeTypeRepository.GetById(
            new ApplicationId(request.ApplicationId),
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSegmentType.DesignPlans },
            cancellationToken);

        try
        {
            var designFile = DesignPlanFileEntity.ForUpload(
                new FileName(request.File.Name),
                new FileSize(request.File.Lenght),
                request.File.Content,
                _documentSettings);
            homeType.DesignPlans.AddFilesToUpload(new[] { designFile });

            await _homeTypeRepository.Save(homeType, new[] { HomeTypeSegmentType.DesignPlans }, cancellationToken);

            var uploadedFile = homeType.DesignPlans.UploadedFiles.Single(x => x.Id == designFile.Id);
            return OperationResult.Success<UploadedFileContract?>(Map(uploadedFile));
        }
        catch (DomainValidationException ex)
        {
            return new OperationResult<UploadedFileContract?>(ex.OperationResult.Errors, null);
        }
    }

    private static UploadedFileContract Map(UploadedFile uploadedFile)
    {
        return new UploadedFileContract(uploadedFile.Id.Value, uploadedFile.Name.Value, uploadedFile.UploadedOn, uploadedFile.UploadedBy, true);
    }
}
