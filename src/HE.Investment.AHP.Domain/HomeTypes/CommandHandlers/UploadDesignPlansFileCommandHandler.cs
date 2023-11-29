using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
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

    public UploadDesignPlansFileCommandHandler(IHomeTypeRepository homeTypeRepository)
    {
        _homeTypeRepository = homeTypeRepository;
    }

    public async Task<OperationResult<UploadedFileContract?>> Handle(UploadDesignPlansFileCommand request, CancellationToken cancellationToken)
    {
        var homeType = await _homeTypeRepository.GetById(
            new ApplicationId(request.ApplicationId),
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSegmentType.DesignPlans },
            cancellationToken);
        DesignPlanFileEntity? designFile = null;

        try
        {
            designFile = DesignPlanFileEntity.ForUpload(new FileName(request.File.Name), new FileSize(request.File.Lenght), request.File.Content);
            homeType.DesignPlans.AddFilesToUpload(new[] { designFile });
        }
        catch (DomainValidationException ex)
        {
            return new OperationResult<UploadedFileContract?>(ex.OperationResult.Errors, null);
        }

        await _homeTypeRepository.Save(homeType, new[] { HomeTypeSegmentType.DesignPlans }, cancellationToken);

        return OperationResult.Success(Map(homeType.DesignPlans.UploadedFiles.Single(x => x.Id == designFile.Id)));
    }

    private static UploadedFileContract? Map(UploadedFile uploadedFile)
    {
        return new UploadedFileContract(uploadedFile.Id.Value, uploadedFile.Name.Value, uploadedFile.UploadedOn, uploadedFile.UploadedBy, true);
    }
}
