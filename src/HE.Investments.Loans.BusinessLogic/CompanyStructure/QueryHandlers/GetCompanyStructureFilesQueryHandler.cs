using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Models.Table;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Constants;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.QueryHandlers;

public class GetCompanyStructureFilesQueryHandler : IRequestHandler<GetCompanyStructureFilesQuery, TableResult<FileTableRow>>
{
    private readonly IHttpDocumentService _documentService;

    private readonly IDocumentServiceConfig _config;

    private readonly ICompanyStructureRepository _repository;

    public GetCompanyStructureFilesQueryHandler(
        IHttpDocumentService documentService,
        IDocumentServiceConfig config,
        ICompanyStructureRepository repository)
    {
        _documentService = documentService;
        _config = config;
        _repository = repository;
    }

    public async Task<TableResult<FileTableRow>> Handle(GetCompanyStructureFilesQuery request, CancellationToken cancellationToken)
    {
        var path = await _repository.GetFilesLocationAsync(request.LoanApplicationId, cancellationToken);

        var folderPaths = new List<string> { $"{path}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}" };

        var result = await _documentService.GetTableRowsAsync(new FileTableFilter()
        {
            ListTitle = _config.ListTitle,
            ListAlias = _config.ListAlias,
            FolderPaths = folderPaths,
        });

        return result;
    }
}
