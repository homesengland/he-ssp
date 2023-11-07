using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;

public class ProvideAdditionalProjectsCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideAdditionalProjectsCommand, OperationResult>
{
    public ProvideAdditionalProjectsCommandHandler(IFundingRepository repository, ILoanUserContext loanUserContext, ILogger<FundingBaseCommandHandler> logger)
        : base(repository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideAdditionalProjectsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var additionalProjects = request.IsThereAnyAdditionalProject.IsProvided() ? AdditionalProjects.FromString(request.IsThereAnyAdditionalProject!) : null;

                x.ProvideAdditionalProjects(additionalProjects);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
