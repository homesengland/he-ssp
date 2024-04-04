using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Loans.BusinessLogic.PrefillData.Repositories;
using HE.Investments.Loans.Contract.PrefillData;
using HE.Investments.Loans.Contract.PrefillData.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.PrefillData.QueryHandlers;

public class GetNewLoanApplicationPrefillDataQueryHandler : IRequestHandler<GetNewLoanApplicationPrefillDataQuery, NewLoanApplicationPrefillData>
{
    private readonly ILoanPrefillDataRepository _prefillDataRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetNewLoanApplicationPrefillDataQueryHandler(ILoanPrefillDataRepository prefillDataRepository, IAccountUserContext accountUserContext)
    {
        _prefillDataRepository = prefillDataRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<NewLoanApplicationPrefillData> Handle(GetNewLoanApplicationPrefillDataQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var prefillData = await _prefillDataRepository.GetLoanApplicationPrefillData(
            new FrontDoorProjectId(request.FrontDoorProjectId),
            userAccount,
            cancellationToken);

        return new NewLoanApplicationPrefillData(prefillData.FundingPurpose, prefillData.Name);
    }
}
