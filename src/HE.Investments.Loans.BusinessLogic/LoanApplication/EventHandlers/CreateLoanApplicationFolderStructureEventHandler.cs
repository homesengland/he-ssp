using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Constants;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
public class CreateLoanApplicationFolderStructureEventHandler : IEventHandler<LoanApplicationHasBeenStartedEvent>
{
    private readonly IDocumentService _documentService;

    private readonly IFileApplicationRepository _fileApplicationRepository;

    private readonly ILoansDocumentSettings _documentSettings;

    public CreateLoanApplicationFolderStructureEventHandler(
        IDocumentService documentService,
        IFileApplicationRepository fileApplicationRepository,
        ILoansDocumentSettings documentSettings)
    {
        _documentService = documentService;
        _fileApplicationRepository = fileApplicationRepository;
        _documentSettings = documentSettings;
    }

    public async Task Handle(LoanApplicationHasBeenStartedEvent domainEvent, CancellationToken cancellationToken)
    {
        var basePath = await _fileApplicationRepository.GetBaseFilePath(domainEvent.LoanApplicationId, cancellationToken);

        await _documentService.CreateFoldersAsync(
            _documentSettings.ListTitle,
            [
                $"{basePath}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}",
                $"{basePath}{CompanyStructureConstants.MoreInformationAboutOrganizationInternal}",
                $"{basePath}/internal/cashflow",
            ],
            cancellationToken);
    }
}
