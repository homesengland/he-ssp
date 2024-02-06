using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.TenderingStatus;

public record ProvideContractorDetailsCommand(
        SiteId SiteId,
        string? ContractorName,
        bool? IsSmeContractor)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
