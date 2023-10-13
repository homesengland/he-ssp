using System.Globalization;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationCommand, OperationResult>
{
    private readonly IAppConfig _appConfig;

    private readonly IHttpDocumentService _documentService;

    public ProvideMoreInformationAboutOrganizationCommandHandler(
                ICompanyStructureRepository repository,
                ILoanUserContext loanUserContext,
                IAppConfig appConfig,
                ILogger<CompanyStructureBaseCommandHandler> logger,
                IHttpDocumentService documentService)
        : base(repository, loanUserContext, logger)
    {
        _appConfig = appConfig;
        _documentService = documentService;
    }

    public async Task<OperationResult> Handle(ProvideMoreInformationAboutOrganizationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            (companyStructure, userAccount) =>
            {
                companyStructure.ProvideMoreInformation(
                    request.OrganisationMoreInformation.IsProvided() ? new OrganisationMoreInformation(request.OrganisationMoreInformation!) : null);

                var maxCount = 10;
                var operationResult = OperationResult.New();
                if (request.OrganisationMoreInformationFiles?.Count > maxCount)
                {
                    operationResult.AddValidationError(nameof(OrganisationMoreInformationFile), string.Format(CultureInfo.InvariantCulture, ValidationErrorMessage.FilesMaxCount, maxCount));
                }

                operationResult.CheckErrors();

                return Task.CompletedTask;
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
