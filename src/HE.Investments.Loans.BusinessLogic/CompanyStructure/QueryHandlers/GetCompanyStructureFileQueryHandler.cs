using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.QueryHandlers;

public class GetCompanyStructureFileQueryHandler : IRequestHandler<GetCompanyStructureFileQuery, FileData>
{
    private readonly IHttpDocumentService _documentService;
    private readonly IDocumentServiceConfig _config;

    public GetCompanyStructureFileQueryHandler(
        IHttpDocumentService documentService,
        IDocumentServiceConfig config)
    {
        _documentService = documentService;
        _config = config;
    }

    public async Task<FileData> Handle(GetCompanyStructureFileQuery request, CancellationToken cancellationToken)
    {
        var result = await _documentService.DownloadAsync(_config.ListAlias, request.FolderPath, request.FileName);

        return result;
    }
}
