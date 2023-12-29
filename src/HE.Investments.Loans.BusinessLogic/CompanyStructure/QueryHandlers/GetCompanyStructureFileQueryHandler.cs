using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.Contract.Common;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.QueryHandlers;

public class GetCompanyStructureFileQueryHandler : IRequestHandler<GetCompanyStructureFileQuery, DownloadedFile>
{
    private readonly IDocumentService _documentService;
    private readonly ILoansDocumentSettings _documentSettings;

    public GetCompanyStructureFileQueryHandler(
        IDocumentService documentService,
        ILoansDocumentSettings documentSettings)
    {
        _documentService = documentService;
        _documentSettings = documentSettings;
    }

    public async Task<DownloadedFile> Handle(GetCompanyStructureFileQuery request, CancellationToken cancellationToken)
    {
        var result = await _documentService.DownloadAsync(
            new FileLocation(_documentSettings.ListTitle, _documentSettings.ListAlias, request.FolderPath),
            request.FileName,
            cancellationToken);

        return new DownloadedFile(result.Name, result.Content);
    }
}
