using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

public class DownloadDesignFileQueryHandler : IRequestHandler<DownloadDesignFileQuery, DownloadedFile>
{
    private readonly IAhpFileService<DesignFileParams> _designFileService;

    public DownloadDesignFileQueryHandler(IAhpFileService<DesignFileParams> designFileService)
    {
        _designFileService = designFileService;
    }

    public async Task<DownloadedFile> Handle(DownloadDesignFileQuery request, CancellationToken cancellationToken)
    {
        var file = await _designFileService.DownloadFile(
            request.FileId,
            new DesignFileParams(request.ApplicationId, new HomeTypeId(request.HomeTypeId)),
            cancellationToken);

        return new DownloadedFile(file.Name, file.Content);
    }
}
