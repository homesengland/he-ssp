using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class ReactivateApplicationCommandHandler : ChangeApplicationStatusBaseCommandHandler, IRequestHandler<ReactivateApplicationCommand, OperationResult>
{
    public ReactivateApplicationCommandHandler(IApplicationRepository applicationRepository, IAccountUserContext accountUserContext)
        : base(applicationRepository, accountUserContext)
    {
    }

    public async Task<OperationResult> Handle(ReactivateApplicationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async (applicationRepository, application, organisationId) =>
            {
                var previousApplicationStatus = ApplicationStatus.Draft; // todo fetch previous status from crm

                await application.Reactivate(applicationRepository, previousApplicationStatus, organisationId, cancellationToken);
            },
            request.Id,
            cancellationToken);
    }
}
