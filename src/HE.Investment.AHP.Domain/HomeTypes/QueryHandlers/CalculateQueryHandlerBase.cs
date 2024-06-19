using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

public abstract class CalculateQueryHandlerBase<TQuery> : IRequestHandler<TQuery, (OperationResult OperationResult, CalculationResult CalculationResult)>
    where TQuery : CalculateQueryBase
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    private readonly IConsortiumUserContext _accountUserContext;

    private readonly ILogger _logger;

    protected CalculateQueryHandlerBase(IHomeTypeRepository homeTypeRepository, IConsortiumUserContext accountUserContext, ILogger logger)
    {
        _homeTypeRepository = homeTypeRepository;
        _accountUserContext = accountUserContext;
        _logger = logger;
    }

    protected abstract IEnumerable<Action<TQuery, IHomeTypeEntity>> CalculateActions { get; }

    public async Task<(OperationResult OperationResult, CalculationResult CalculationResult)> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeType = await _homeTypeRepository.GetById(
            request.ApplicationId,
            request.HomeTypeId,
            account,
            cancellationToken);

        homeType.TenureDetails.ClearValuesForNewCalculation();

        var errors = PerformWithValidation(CalculateActions
            .Select<Action<TQuery, IHomeTypeEntity>, Action>(x => () => x(request, homeType))
            .ToArray());

        var operationResult = errors.Count != 0 ? new OperationResult(errors) : OperationResult.Success();
        var calculationResult = BuildCalculationResult(homeType);

        return (operationResult, calculationResult);
    }

    protected abstract CalculationResult BuildCalculationResult(IHomeTypeEntity homeType);

    private List<ErrorItem> PerformWithValidation(params Action[] actions)
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
