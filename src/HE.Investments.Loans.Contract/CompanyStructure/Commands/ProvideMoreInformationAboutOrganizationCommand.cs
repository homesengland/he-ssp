using HE.Investments.Common.Validators;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.Loans.Contract.CompanyStructure.Commands;

public record ProvideMoreInformationAboutOrganizationCommand(
                LoanApplicationId LoanApplicationId,
                string? OrganisationMoreInformation,
                IList<FileTableRow>? OrganisationMoreInformationFiles,
                List<IFormFile>? FormFiles) : IRequest<OperationResult>;
