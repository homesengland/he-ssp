using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

public abstract class CalculateQueryHandlerBase<TQuery> : IRequestHandler<TQuery, (OperationResult OperationResult, CalculationResult CalculationResult)>
    where TQuery : CalculateQueryBase
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    private readonly ILogger _logger;

    protected CalculateQueryHandlerBase(IHomeTypeRepository homeTypeRepository, ILogger logger)
    {
        _homeTypeRepository = homeTypeRepository;
        _logger = logger;
    }

    protected abstract IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes { get; }

    protected abstract IEnumerable<Action<TQuery, IHomeTypeEntity>> CalculateActions { get; }

    public async Task<(OperationResult OperationResult, CalculationResult CalculationResult)> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var applicationId = new ApplicationId(request.ApplicationId);
        var homeType = await _homeTypeRepository.GetById(
            applicationId,
            new HomeTypeId(request.HomeTypeId),
            SegmentTypes,
            cancellationToken);

        var errors = PerformWithValidation(CalculateActions
            .Select<Action<TQuery, IHomeTypeEntity>, Action>(x => () => x(request, homeType))
            .ToArray());

        var operationResult = errors.Any() ? new OperationResult(errors) : OperationResult.Success();
        var calculationResult = BuildCalculationResult(homeType);

        return (operationResult, calculationResult);
    }

    protected abstract CalculationResult BuildCalculationResult(IHomeTypeEntity homeType);

    private IList<ErrorItem> PerformWithValidation(params Action[] actions)
    {
        var errors = new List<ErrorItem>();
        foreach (var action in actions)
        {
            try
            {
                action();
            }
            catch (DomainValidationException domainValidationException)
            {
                _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
                errors.AddRange(domainValidationException.OperationResult.Errors);
            }
        }

        return errors;
    }
}
