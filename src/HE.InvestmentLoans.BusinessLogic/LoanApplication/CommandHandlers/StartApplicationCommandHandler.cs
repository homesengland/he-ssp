using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Constants;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public class StartApplicationCommandHandler : IRequestHandler<StartApplicationCommand, OperationResult<LoanApplicationId?>>
{
    private readonly ILoanUserContext _loanUserContext;
    private readonly ILoanApplicationRepository _applicationRepository;
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly ILogger<StartApplicationCommandHandler> _logger;
    private readonly IDocumentServiceConfig _documentServiceConfig;
    private readonly IHttpDocumentService _documentService;
    private readonly ICompanyStructureRepository _companyStructureRepository;

    public StartApplicationCommandHandler(
        ILoanUserContext loanUserContext,
        ILoanApplicationRepository applicationRepository,
        ILogger<StartApplicationCommandHandler> logger,
        IApplicationProjectsRepository applicationProjectsRepository,
        IDocumentServiceConfig documentServiceConfig,
        IHttpDocumentService documentService,
        ICompanyStructureRepository companyStructureRepository)
    {
        _loanUserContext = loanUserContext;
        _applicationRepository = applicationRepository;
        _logger = logger;
        _applicationProjectsRepository = applicationProjectsRepository;
        _documentServiceConfig = documentServiceConfig;
        _documentService = documentService;
        _companyStructureRepository = companyStructureRepository;
    }

    public async Task<OperationResult<LoanApplicationId?>> Handle(StartApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userAccount = await _loanUserContext.GetSelectedAccount();

            var applicationName = new LoanApplicationName(request.ApplicationName);
            var newLoanApplication = LoanApplicationEntity.New(userAccount, applicationName);

            if (await _applicationRepository.IsExist(applicationName, userAccount, cancellationToken))
            {
                return new OperationResult<LoanApplicationId?>(
                    new[] { new ErrorItem(nameof(LoanApplicationName), "This name has been used for another application") },
                    null);
            }

            await _applicationRepository.Save(newLoanApplication, await _loanUserContext.GetUserDetails(), cancellationToken);

            var applicationProjects = new ApplicationProjects(newLoanApplication.Id);
            await _applicationProjectsRepository.SaveAllAsync(applicationProjects, userAccount, cancellationToken);

            var filesLocation = await _companyStructureRepository.GetFilesLocationAsync(newLoanApplication.Id, cancellationToken);
            await _documentService.CreateFoldersAsync(_documentServiceConfig.ListTitle, new List<string>
            {
                $"{filesLocation}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}",
                $"{filesLocation}{CompanyStructureConstants.MoreInformationAboutOrganizationInternal}",
            });

            return OperationResult.Success<LoanApplicationId?>(newLoanApplication.Id);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);

            return new OperationResult<LoanApplicationId?>(domainValidationException.OperationResult.Errors, null);
        }
    }
}
