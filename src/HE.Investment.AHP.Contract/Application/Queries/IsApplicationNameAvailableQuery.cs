using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Application.Queries;

public record IsApplicationNameAvailableQuery(string? ApplicationName) : IRequest<OperationResult>;
