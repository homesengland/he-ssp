using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.CompanyStructure.Commands;

public record ProvideMoreInformationAboutOrganizationRemoveFileCommand(LoanApplicationId LoanApplicationId, string FileName) : IRequest<OperationResult>;
