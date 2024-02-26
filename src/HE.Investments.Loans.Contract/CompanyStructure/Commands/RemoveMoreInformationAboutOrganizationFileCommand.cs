using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.CompanyStructure.Commands;

public record RemoveMoreInformationAboutOrganizationFileCommand(LoanApplicationId LoanApplicationId, FileId FileId) : IRequest<OperationResult>;
