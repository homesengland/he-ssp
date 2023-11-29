using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Application.Queries;

public record GetApplicationDashboardQuery(LoanApplicationId ApplicationId) : IRequest<GetApplicationDashboardQueryResponse>;

public record GetApplicationDashboardQueryResponse(
    LoanApplicationId ApplicationId,
    LoanApplicationName ApplicationName,
    ApplicationStatus ApplicationStatus,
    string ApplicationReferenceNumber,
    string OrganizationName,
    DateTime? LastEditedOn,
    string LastEditedBy);
