using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class SubmitApplicationCommandHandler : ApplicationCommandHandlerBase, IRequestHandler<SubmitApplicationCommand, OperationResult>
{
    public SubmitApplicationCommandHandler(IApplicationRepository applicationRepository, IConsortiumUserContext accountUserContext)
        : base(applicationRepository, accountUserContext)
    {
    }

    public async Task<OperationResult> Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            application =>
            {
                application.Submit(RepresentationsAndWarranties.FromString(request.RepresentationsAndWarranties));

                return Task.CompletedTask;
            },
            request.Id,
            cancellationToken);
    }
}
