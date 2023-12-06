using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Loans.Common.Exceptions;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.QueryHandlers;

public class GetStakeholderDiscussionsFileQueryHandler : IRequestHandler<GetStakeholderDiscussionsFileQuery, DownloadedFile>
{
    private readonly IAhpFileService<LocalAuthoritySupportFileParams> _fileService;

    public GetStakeholderDiscussionsFileQueryHandler(IAhpFileService<LocalAuthoritySupportFileParams> fileService)
    {
        _fileService = fileService;
    }

    public async Task<DownloadedFile> Handle(GetStakeholderDiscussionsFileQuery request, CancellationToken cancellationToken)
    {
        var file = await _fileService.DownloadFile(
            new FileId(request.FileId),
            new LocalAuthoritySupportFileParams(new(request.ApplicationId)),
            cancellationToken);

        return new DownloadedFile(file.Name, file.Content);
    }
}
