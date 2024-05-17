using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Commands;

public record ProvideApplicationPartnersConfirmationCommand(AhpApplicationId ApplicationId, bool? IsConfirmed) : IRequest<OperationResult>, IUpdateSchemeCommand;
