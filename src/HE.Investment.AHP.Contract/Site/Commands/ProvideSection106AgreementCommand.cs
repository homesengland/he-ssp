using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSection106AgreementCommand(string SiteId, bool? Agreement) : IRequest<OperationResult>;
