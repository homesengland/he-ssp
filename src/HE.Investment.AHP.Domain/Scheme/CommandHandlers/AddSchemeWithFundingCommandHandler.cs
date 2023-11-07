using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class AddSchemeWithFundingCommandHandler : IRequestHandler<AddSchemeWithFundingCommand, OperationResult<SchemeId?>>
{
    private readonly ISchemeRepository _repository;

    public AddSchemeWithFundingCommandHandler(ISchemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<SchemeId?>> Handle(AddSchemeWithFundingCommand request, CancellationToken cancellationToken)
    {
        var scheme = new SchemeEntity(
            new SchemeId(Guid.NewGuid().ToString()),
            new SchemeFunding(request.RequiredFunding, request.HousesToDeliver));

        await _repository.Save(scheme, cancellationToken);

        return new OperationResult<SchemeId?>(scheme.Id);
    }
}
