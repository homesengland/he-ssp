using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.InvestmentLoans.Contract.Domain;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record ProvideMoreInformationAboutOrganizationCommand(
                LoanApplicationId LoanApplicationId,
                Providable<OrganisationMoreInformation> OrganisationMoreInformation,
                Providable<OrganisationMoreInformationFile> OrganisationMoreInformationFile) : IRequest;
