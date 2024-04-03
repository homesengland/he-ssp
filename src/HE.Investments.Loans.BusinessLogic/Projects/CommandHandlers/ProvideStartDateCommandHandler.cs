using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvideStartDateCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideStartDateCommand, OperationResult>
{
    public ProvideStartDateCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvideStartDateCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                if (request.Exists.IsNotProvided())
                {
                    return;
                }

                project.ProvideStartDate(new StartDate(request.Exists.MapToNonNullableBool(), request.Day, request.Month, request.Year));
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
