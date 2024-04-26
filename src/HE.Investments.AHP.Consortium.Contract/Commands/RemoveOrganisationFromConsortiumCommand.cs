using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Commands;

public record RemoveOrganisationFromConsortiumCommand(ConsortiumId ConsortiumId, string OrganisationId, bool? IsConfirmed) : IRequest<OperationResult>;
