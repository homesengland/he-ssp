using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Mappers;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.CompanyStructure.Queries;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Models.Table;
using HE.Investments.DocumentService.Services;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.QueryHandlers;

public class GetCompanyStructureFilesQueryHandler : IRequestHandler<GetCompanyStructureFilesQuery, TableResult<FileTableRow>>
{
    private readonly IHttpDocumentService _documentService;
    private readonly IDocumentServiceConfig _config;

    public GetCompanyStructureFilesQueryHandler(
        IHttpDocumentService documentService,
        IDocumentServiceConfig config)
    {
        _documentService = documentService;
        _config = config;
    }

    public async Task<TableResult<FileTableRow>> Handle(GetCompanyStructureFilesQuery request, CancellationToken cancellationToken)
    {
        var folderPath = "0000000_DA2123DAE440EE11BDF3002248C653E1";

        var result = await _documentService.GetTableRowsAsync(new FileTableFilter()
        {
            ListTitle = _config.ListTitle,
            ListAlias = _config.ListAlias,
            FolderPath = folderPath,
        });

        return result;
    }
}
