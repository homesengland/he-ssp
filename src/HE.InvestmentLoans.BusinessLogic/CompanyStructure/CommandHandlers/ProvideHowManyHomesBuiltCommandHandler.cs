using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideHowManyHomesBuiltCommandHandler :
    CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideHowManyHomesBuiltCommand, OperationResult>
{
    public ProvideHowManyHomesBuiltCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext, ILogger<CompanyStructureBaseCommandHandler> logger)
        : base(repository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideHowManyHomesBuiltCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            companyStructure =>
            {
                var homesBuild = request.HomesBuilt.IsProvided() ? HomesBuilt.FromString(request.HomesBuilt!) : null;
                companyStructure.ProvideHowManyHomesBuilt(homesBuild);

                return Task.CompletedTask;
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
