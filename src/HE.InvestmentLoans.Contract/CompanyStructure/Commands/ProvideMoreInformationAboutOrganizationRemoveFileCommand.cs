using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record ProvideMoreInformationAboutOrganizationRemoveFileCommand(
                LoanApplicationId LoanApplicationId,
                string FolderPath,
                string FileName) : IRequest<OperationResult>;
