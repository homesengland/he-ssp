using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeFundingCommandHandler : IRequestHandler<ChangeSchemeFundingCommand, OperationResult>
{
    private readonly ISchemeRepository _repository;

    public ChangeSchemeFundingCommandHandler(ISchemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult> Handle(ChangeSchemeFundingCommand request, CancellationToken cancellationToken)
    {
        var applicationId = new DomainApplicationId(request.ApplicationId);
        var funding = new SchemeFunding(request.RequiredFunding, request.HousesToDeliver);

        try
        {
            var scheme = await _repository.GetById(applicationId, cancellationToken);
            scheme!.ChangeFunding(funding);
            await _repository.Save(applicationId, scheme, cancellationToken);
        }
        catch (NotFoundException)
        {
            var scheme = new SchemeEntity(funding);
            await _repository.Save(applicationId, scheme, cancellationToken);
        }

        return OperationResult.Success();
    }
}
