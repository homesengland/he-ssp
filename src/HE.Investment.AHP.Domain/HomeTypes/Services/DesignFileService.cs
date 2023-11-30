using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Services;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.DocumentService.Models;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.Services;

public class DesignFileService : IDesignFileService
{
    private readonly IAhpFileService _ahpFileService;

    public DesignFileService(IAhpFileService ahpFileService)
    {
        _ahpFileService = ahpFileService;
    }

    public async Task<IReadOnlyCollection<UploadedFile>> GetFiles(ApplicationId applicationId, HomeTypeId homeTypeId, CancellationToken cancellationToken)
    {
        var fileLocation = GetDesignFileLocation(applicationId, homeTypeId);
        return await _ahpFileService.GetFiles(fileLocation, cancellationToken);
    }

    public async Task<UploadedFile> UploadFile(ApplicationId applicationId, HomeTypeId homeTypeId, FileName name, Stream content, CancellationToken cancellationToken)
    {
        var fileLocation = GetDesignFileLocation(applicationId, homeTypeId);
        return await _ahpFileService.UploadFile(name, content, fileLocation, cancellationToken);
    }

    public async Task RemoveFile(ApplicationId applicationId, HomeTypeId homeTypeId, FileId fileId, CancellationToken cancellationToken)
    {
        var fileLocation = GetDesignFileLocation(applicationId, homeTypeId);
        await _ahpFileService.RemoveFile(fileId, fileLocation, cancellationToken);
    }

    private static FileLocation GetDesignFileLocation(ApplicationId applicationId, HomeTypeId homeTypeId)
    {
        // TODO: Load application Number and Home Type number from CRM
        // TODO: create directories when creating Application/HomeType
        return new FileLocation("AHP Application", "invln_scheme", $"{applicationId}/Home Type/{homeTypeId}/external/Design Files");
    }
}
