using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;
public record ProvideLocationCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string TypeOfLocation, string Coordinates, string LandregistryTitleNumber) : IRequest<OperationResult>;
