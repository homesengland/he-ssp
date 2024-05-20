using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Commands;

public record ProvideOwnerOfTheLandCommand(AhpApplicationId ApplicationId, OrganisationId OrganisationId, bool? IsPartnerConfirmed) : IRequest<OperationResult>, IUpdateSchemeCommand;
