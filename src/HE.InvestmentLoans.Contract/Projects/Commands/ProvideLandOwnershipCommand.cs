using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideLandOwnershipCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string ApplicantHasFullOwnership) : IRequest<OperationResult>;
