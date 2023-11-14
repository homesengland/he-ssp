using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Constants;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Services;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationHasBeenStartedEventHandler : IEventHandler<LoanApplicationHasBeenStartedEvent>
{
    private readonly IHttpDocumentService _documentService;

    private readonly ICompanyStructureRepository _companyStructureRepository;

    private readonly IDocumentServiceConfig _documentServiceConfig;

    public LoanApplicationHasBeenStartedEventHandler(
        IHttpDocumentService documentService,
        ICompanyStructureRepository companyStructureRepository,
        IDocumentServiceConfig documentServiceConfig)
    {
        _documentService = documentService;
        _companyStructureRepository = companyStructureRepository;
        _documentServiceConfig = documentServiceConfig;
    }

    public async Task Handle(LoanApplicationHasBeenStartedEvent domainEvent, CancellationToken cancellationToken)
    {
        var filesLocation = await _companyStructureRepository.GetFilesLocationAsync(new LoanApplicationId(domainEvent.LoanApplicationId), cancellationToken);
        await _documentService.CreateFoldersAsync(_documentServiceConfig.ListTitle, new List<string>
        {
            $"{filesLocation}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}",
            $"{filesLocation}{CompanyStructureConstants.MoreInformationAboutOrganizationInternal}",
        });
    }
}
