using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Application.Queries;

public record GetApplicationDashboardQuery(LoanApplicationId ApplicationId) : IRequest<GetApplicationDashboardQueryResponse>;

public record GetApplicationDashboardQueryResponse(
    LoanApplicationId ApplicationId,
    string ApplicationName,
    ApplicationStatus ApplicationStatus,
    string ApplicationReferenceNumber,
    string OrganizationName);
