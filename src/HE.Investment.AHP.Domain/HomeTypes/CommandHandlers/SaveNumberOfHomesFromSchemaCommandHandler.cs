using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveNumberOfHomesFromSchemaCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<SaveNumberOfHomesFromSchemaCommand, OperationResult>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public SaveNumberOfHomesFromSchemaCommandHandler(IHomeTypeRepository repository, ISchemeRepository schemeRepository, IAccountUserContext accountUserContext, ILogger<SaveFinishHomeTypesAnswerCommandHandler> logger)
        : base(logger)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(SaveNumberOfHomesFromSchemaCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeTypes = await _repository.GetByApplicationId(request.ApplicationId, account, HomeTypeSegmentTypes.HomeInformationOnly, cancellationToken);
        var validationErrors = PerformWithValidation(() => homeTypes.ChangeNumberOfHomesFromSchemaSection(request.NumberOfHomesFromSchemaSection));
        if (validationErrors.Any())
        {
            return new OperationResult(validationErrors);
        }

        await _repository.Save(homeTypes, account.SelectedOrganisationId(), cancellationToken);
        return OperationResult.Success();
    }
}
