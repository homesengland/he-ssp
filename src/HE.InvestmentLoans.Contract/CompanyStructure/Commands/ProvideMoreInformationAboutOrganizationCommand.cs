using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record ProvideMoreInformationAboutOrganizationCommand(
                LoanApplicationId LoanApplicationId,
                string? OrganisationMoreInformation,
                string? FileName,
                byte[]? FileContent) : IRequest<OperationResult>;
