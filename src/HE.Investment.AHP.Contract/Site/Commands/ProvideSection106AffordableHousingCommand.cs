using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSection106AffordableHousingCommand(SiteId SiteId, bool? AffordableHousing) : IRequest<OperationResult>;
