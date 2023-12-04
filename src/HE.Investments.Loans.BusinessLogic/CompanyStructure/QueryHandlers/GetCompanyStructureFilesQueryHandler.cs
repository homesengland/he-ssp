using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Constants;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using HE.Investments.Loans.Contract.Documents;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.QueryHandlers;

public class GetCompanyStructureFilesQueryHandler : IRequestHandler<GetCompanyStructureFilesQuery, LoansTableResult>
{
    private readonly IDocumentService _documentService;

    private readonly ILoansDocumentSettings _documentSettings;

    private readonly ICompanyStructureRepository _repository;

    public GetCompanyStructureFilesQueryHandler(
        IDocumentService documentService,
        ILoansDocumentSettings documentSettings,
        ICompanyStructureRepository repository)
    {
        _documentService = documentService;
        _documentSettings = documentSettings;
        _repository = repository;
    }

    public async Task<LoansTableResult> Handle(GetCompanyStructureFilesQuery request, CancellationToken cancellationToken)
    {
        var path = await _repository.GetFilesLocationAsync(request.LoanApplicationId, cancellationToken);

        var folderPaths = new List<string> { $"{path}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}" };

        var result = await _documentService.GetFilesAsync<LoansFileMetadata>(
            new GetFilesQuery(_documentSettings.ListTitle, _documentSettings.ListAlias, folderPaths),
            cancellationToken);

        return new LoansTableResult
        {
            Items = result.Select(
                    x => new LoansFileTableRow
                    {
                        FileName = x.FileName,
                        FolderPath = x.FolderPath,
                        Editor = x.Editor,
                        Modified = x.Modified,
                        Creator = x.Metadata?.Creator,
                    })
                .ToList(),
        };
    }
}
