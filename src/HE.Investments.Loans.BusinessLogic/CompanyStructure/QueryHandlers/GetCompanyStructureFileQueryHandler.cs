using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.QueryHandlers;

public class GetCompanyStructureFileQueryHandler : IRequestHandler<GetCompanyStructureFileQuery, DownloadFileData>
{
    private readonly IDocumentService _documentService;
    private readonly IDocumentServiceConfig _config;

    public GetCompanyStructureFileQueryHandler(
        IDocumentService documentService,
        IDocumentServiceConfig config)
    {
        _documentService = documentService;
        _config = config;
    }

    public async Task<DownloadFileData> Handle(GetCompanyStructureFileQuery request, CancellationToken cancellationToken)
    {
        var result = await _documentService.DownloadAsync(
            new FileLocation(_config.ListTitle, _config.ListAlias, request.FolderPath),
            request.FileName,
            cancellationToken);

        return result;
    }
}
