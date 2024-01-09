using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.Loans.Contract.CompanyStructure.Commands;

public record ProvideMoreInformationAboutOrganizationCommand(
                LoanApplicationId LoanApplicationId,
                string? OrganisationMoreInformation,
                IList<LoansFileTableRow>? OrganisationMoreInformationFiles,
                List<IFormFile>? FormFiles) : IRequest<OperationResult>;
