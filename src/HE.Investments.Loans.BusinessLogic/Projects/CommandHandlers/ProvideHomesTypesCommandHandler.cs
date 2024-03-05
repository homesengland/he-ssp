using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvideHomesTypesCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideHomesTypesCommand, OperationResult>
{
    public ProvideHomesTypesCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvideHomesTypesCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                if (request.HomesTypes.IsNotProvided())
                {
                    return;
                }

                project.ProvideHomesTypes(new HomesTypes(request.HomesTypes, request.OtherHomesTypes));
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
