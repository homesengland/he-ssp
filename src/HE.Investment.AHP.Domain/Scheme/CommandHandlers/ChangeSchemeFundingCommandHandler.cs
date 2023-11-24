using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeFundingCommandHandler : IRequestHandler<ChangeSchemeFundingCommand, OperationResult>
{
    private readonly ISchemeRepository _repository;
    private readonly IApplicationRepository _applicationRepository;

    public ChangeSchemeFundingCommandHandler(ISchemeRepository repository, IApplicationRepository applicationRepository)
    {
        _repository = repository;
        _applicationRepository = applicationRepository;
    }

    public async Task<OperationResult> Handle(ChangeSchemeFundingCommand request, CancellationToken cancellationToken)
    {
        var applicationId = new DomainApplicationId(request.ApplicationId);
        var funding = new SchemeFunding(request.RequiredFunding, request.HousesToDeliver);

        try
        {
            var scheme = await _repository.GetByApplicationId(applicationId, cancellationToken);
            scheme.ChangeFunding(funding);
            await _repository.Save(scheme, cancellationToken);
        }
        catch (NotFoundException)
        {
            var application = await _applicationRepository.GetById(new DomainApplicationId(request.ApplicationId), cancellationToken);
            var applicationBasic = new ApplicationBasicDetails(new DomainApplicationId(request.ApplicationId), application.Name);
            var scheme = new SchemeEntity(applicationBasic, funding);
            await _repository.Save(scheme, cancellationToken);
        }

        return OperationResult.Success();
    }
}
