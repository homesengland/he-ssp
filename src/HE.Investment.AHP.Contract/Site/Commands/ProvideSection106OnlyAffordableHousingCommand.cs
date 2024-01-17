using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSection106OnlyAffordableHousingCommand(SiteId SiteId, bool? OnlyAffordableHousing) : IRequest<OperationResult>;
