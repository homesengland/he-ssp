using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Mappers;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.CompanyStructure.Queries;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Services;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.QueryHandlers;

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
