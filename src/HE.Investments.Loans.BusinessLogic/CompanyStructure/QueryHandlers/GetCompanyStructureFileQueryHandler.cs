using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Constants;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.QueryHandlers;

public class GetCompanyStructureFileQueryHandler : IRequestHandler<GetCompanyStructureFileQuery, DownloadFileData>
{
    private readonly ICompanyStructureRepository _companyStructureRepository;
    private readonly IDocumentService _documentService;
    private readonly ILoansDocumentSettings _documentSettings;

    public GetCompanyStructureFileQueryHandler(
        ICompanyStructureRepository companyStructureRepository,
        IDocumentService documentService,
        ILoansDocumentSettings documentSettings)
    {
        _companyStructureRepository = companyStructureRepository;
        _documentService = documentService;
        _documentSettings = documentSettings;
    }

    public async Task<DownloadFileData> Handle(GetCompanyStructureFileQuery request, CancellationToken cancellationToken)
    {
        var folderPath = $"{await _companyStructureRepository.GetFilesLocationAsync(new LoanApplicationId(request.LoanApplicationId), cancellationToken)}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}";
        var result = await _documentService.DownloadAsync(
            new FileLocation(_documentSettings.ListTitle, _documentSettings.ListAlias, folderPath),
            request.FileName,
            cancellationToken);

        return result;
    }
}
