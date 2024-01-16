using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideSection106AffordableHousingCommand(string SiteId, bool? AffordableHousing) : IRequest<OperationResult>;
