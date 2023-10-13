using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.DocumentService.Models.File;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record ProvideMoreInformationAboutOrganizationFileCommand(
                LoanApplicationId LoanApplicationId,
                List<FileTableRow>? OrganisationMoreInformationFiles,
                IFormFile FormFile) : IRequest<OperationResult>;
