using MediatR;

namespace HE.Investments.Loans.Contract.PrefillData.Queries;

public record GetNewLoanApplicationPrefillDataQuery(string FrontDoorProjectId) : IRequest<NewLoanApplicationPrefillData>;
