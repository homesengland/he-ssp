using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Constants;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationHasBeenStartedEventHandler : IEventHandler<LoanApplicationHasBeenStartedEvent>
{
    private readonly IDocumentService _documentService;

    private readonly ICompanyStructureRepository _companyStructureRepository;

    private readonly ILoansDocumentSettings _documentSettings;

    public LoanApplicationHasBeenStartedEventHandler(
        IDocumentService documentService,
        ICompanyStructureRepository companyStructureRepository,
        ILoansDocumentSettings documentSettings)
    {
        _documentService = documentService;
        _companyStructureRepository = companyStructureRepository;
        _documentSettings = documentSettings;
    }

    public async Task Handle(LoanApplicationHasBeenStartedEvent domainEvent, CancellationToken cancellationToken)
    {
        var filesLocation = await _companyStructureRepository.GetFilesLocationAsync(new LoanApplicationId(domainEvent.LoanApplicationId), cancellationToken);
        await _documentService.CreateFoldersAsync(
            _documentSettings.ListTitle,
            new List<string>
            {
                $"{filesLocation}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}",
                $"{filesLocation}{CompanyStructureConstants.MoreInformationAboutOrganizationInternal}",
            },
            cancellationToken);
    }
}
