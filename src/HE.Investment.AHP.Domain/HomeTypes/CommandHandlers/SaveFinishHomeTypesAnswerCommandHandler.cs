using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveFinishHomeTypesAnswerCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<SaveFinishHomeTypesAnswerCommand, OperationResult>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly ISchemeRepository _schemeRepository;

    public SaveFinishHomeTypesAnswerCommandHandler(IHomeTypeRepository repository, ISchemeRepository schemeRepository, IAccountUserContext accountUserContext, ILogger<SaveFinishHomeTypesAnswerCommandHandler> logger)
        : base(logger)
    {
        _repository = repository;
        _schemeRepository = schemeRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(SaveFinishHomeTypesAnswerCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeTypes = await _repository.GetByApplicationId(request.ApplicationId, account, HomeTypeSegmentTypes.None, cancellationToken);
        var scheme = await _schemeRepository.GetByApplicationId(request.ApplicationId, account, false, cancellationToken);

        if (request.FinishHomeTypesAnswer == Contract.HomeTypes.Enums.FinishHomeTypesAnswer.Yes
            && !request.IsCheckOnly
            && scheme?.Funding?.HousesToDeliver != homeTypes.HomeTypes.Sum(x => x.HomeInformation?.NumberOfHomes?.Value))
        {
            return OperationResult.New().AddValidationError("HomeTypes", "You have not assigned all of the homes you are delivering to a home type");
        }

        var validationErrors = PerformWithValidation(() => homeTypes.CompleteSection(request.FinishHomeTypesAnswer));
        if (validationErrors.Any())
        {
            return new OperationResult(validationErrors);
        }

        if (!request.IsCheckOnly)
        {
            await _repository.Save(homeTypes, account.SelectedOrganisationId(), cancellationToken);
        }

        return OperationResult.Success();
    }
}
