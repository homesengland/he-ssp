using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideHowManyHomesBuiltCommandHandler :
    CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideHowManyHomesBuiltCommand, OperationResult>
{
    public ProvideHowManyHomesBuiltCommandHandler(ICompanyStructureRepository companyStructureRepository, ILoanApplicationRepository loanApplicationRepository, IAccountUserContext loanUserContext, ILogger<CompanyStructureBaseCommandHandler> logger)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext, logger)
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
