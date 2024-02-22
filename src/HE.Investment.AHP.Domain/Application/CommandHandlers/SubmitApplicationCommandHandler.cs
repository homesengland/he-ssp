using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class SubmitApplicationCommandHandler : ChangeApplicationStatusBaseCommandHandler, IRequestHandler<SubmitApplicationCommand, OperationResult>
{
    public SubmitApplicationCommandHandler(IApplicationRepository applicationRepository, IAccountUserContext accountUserContext)
        : base(applicationRepository, accountUserContext)
    {
    }

    public async Task<OperationResult> Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
    {
        var representationsAndWarranties = new RepresentationsAndWarranties(request.RepresentationsAndWarranties);

        return await Perform(
            async (applicationRepository, application, organisationId) =>
            {
                await application.Submit(applicationRepository, organisationId, representationsAndWarranties, cancellationToken);
            },
            request.Id,
            cancellationToken);
    }
}
