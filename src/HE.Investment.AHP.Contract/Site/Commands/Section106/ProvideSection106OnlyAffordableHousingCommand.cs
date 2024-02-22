using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.Section106;

public record ProvideSection106OnlyAffordableHousingCommand(SiteId SiteId, bool? OnlyAffordableHousing)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
