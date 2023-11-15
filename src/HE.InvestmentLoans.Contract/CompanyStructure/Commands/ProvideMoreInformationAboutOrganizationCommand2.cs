using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record ProvideMoreInformationAboutOrganizationCommand2(
    LoanApplicationId LoanApplicationId,
    string? OrganisationMoreInformation,
    IAsyncEnumerable<LargeFile> Files) : IRequest<OperationResult>;
