using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationCommand, OperationResult>
{
    private readonly IAppConfig _appConfig;

    public ProvideMoreInformationAboutOrganizationCommandHandler(
                ICompanyStructureRepository repository,
                ILoanUserContext loanUserContext,
                IAppConfig appConfig,
                ILogger<CompanyStructureBaseCommandHandler> logger)
        : base(repository, loanUserContext, logger)
    {
        _appConfig = appConfig;
    }

    public async Task<OperationResult> Handle(ProvideMoreInformationAboutOrganizationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                x.ProvideMoreInformation(
                    request.OrganisationMoreInformation.IsProvided() ? new OrganisationMoreInformation(request.OrganisationMoreInformation!) : null);

                var moreInformationFile = request.FileName.IsProvided() && request.FileContent.IsProvided()
                    ? new OrganisationMoreInformationFile(request.FileName!, request.FileContent!, _appConfig.MaxFileSizeInMegabytes)
                    : null;

                x.ProvideFileWithMoreInformation(moreInformationFile);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
