using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSection106AgreementCommand(SiteId SiteId, bool? Agreement) : IRequest<OperationResult>;
