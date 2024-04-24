using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Commands;

public record AddOrganisationToConsortiumCommand(ConsortiumId ConsortiumId, string? OrganisationId, string? CompanyHouseNumber) : IRequest<OperationResult>;
