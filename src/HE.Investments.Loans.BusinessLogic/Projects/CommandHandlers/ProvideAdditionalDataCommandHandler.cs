using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Utils;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Common;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvideAdditionalDataCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideAdditionalDetailsCommand, OperationResult>
{
    private readonly IDateTimeProvider _timeProvider;

    public ProvideAdditionalDataCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        IDateTimeProvider timeProvider)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext)
    {
        _timeProvider = timeProvider;
    }

    public async Task<OperationResult> Handle(ProvideAdditionalDetailsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                var aggregatedResults = OperationResult.New();

                var purchaseDate = aggregatedResults.CatchResult(() =>
                    PurchaseDate.FromString(request.PurchaseYear, request.PurchaseMonth, request.PurchaseDay, _timeProvider.Now));

                var cost = aggregatedResults.CatchResult(() => Pounds.FromString(request.Cost));
                aggregatedResults.OverrideError(
                    GenericValidationError.InvalidPoundsValue,
                    nameof(ProjectViewModel.Cost),
                    ValidationErrorMessage.IncorrectProjectCost);

                var currentValue = aggregatedResults.CatchResult(() => Pounds.FromString(request.CurrentValue));
                aggregatedResults.OverrideError(
                    GenericValidationError.InvalidPoundsValue,
                    nameof(ProjectViewModel.Value),
                    ValidationErrorMessage.IncorrectProjectValue);

                var sourceOfValuation = SourceOfValuationMapper.FromString(request.SourceOfValuation);

                if (sourceOfValuation.IsNotProvided())
                {
                    aggregatedResults.AddValidationError(nameof(ProjectViewModel.Source), ValidationErrorMessage.EnterMoreDetails);
                }

                aggregatedResults.CheckErrors();

                project.ProvideAdditionalData(new AdditionalDetails(purchaseDate, cost, currentValue, sourceOfValuation!.Value));
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
