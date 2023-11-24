using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Security.Queries;
public record GetSecurity(LoanApplicationId Id, SecurityFieldsSet SecurityFieldsSet) : IRequest<GetSecurityResponse>;

public record GetSecurityResponse(SecurityViewModel ViewModel);
