using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Loans.Contract.Application.Commands;

public record ConfirmInformationAgreementCommand(string? InformationAgreement) : IRequest<OperationResult>;
