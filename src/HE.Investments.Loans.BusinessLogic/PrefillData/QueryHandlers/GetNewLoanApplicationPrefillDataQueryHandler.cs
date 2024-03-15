using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.Contract.PrefillData;
using HE.Investments.Loans.Contract.PrefillData.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.PrefillData.QueryHandlers;

public class GetNewLoanApplicationPrefillDataQueryHandler : IRequestHandler<GetNewLoanApplicationPrefillDataQuery, NewLoanApplicationPrefillData>
{
    private readonly IPrefillDataRepository _prefillDataRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetNewLoanApplicationPrefillDataQueryHandler(IPrefillDataRepository prefillDataRepository, IAccountUserContext accountUserContext)
    {
        _prefillDataRepository = prefillDataRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<NewLoanApplicationPrefillData> Handle(GetNewLoanApplicationPrefillDataQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var prefillData = await _prefillDataRepository.GetProjectPrefillData(
            new FrontDoorProjectId(request.FrontDoorProjectId),
            userAccount,
            cancellationToken);

        return new NewLoanApplicationPrefillData(
                MapToFundingPurpose(prefillData.SupportActivities),
                prefillData.Name);
    }

    private static FundingPurpose? MapToFundingPurpose(IReadOnlyCollection<SupportActivityType> supportActivities)
    {
        if (supportActivities.Count == 1 && supportActivities.Single() == SupportActivityType.DevelopingHomes)
        {
            return FundingPurpose.BuildingNewHomes;
        }

        return null;
    }
}
