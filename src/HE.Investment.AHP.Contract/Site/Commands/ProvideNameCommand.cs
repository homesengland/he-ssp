using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideNameCommand(string? SiteId, string? Name) : IRequest<OperationResult>;
