﻿using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class CompleteHomeTypeCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<CompleteHomeTypeCommand, OperationResult>
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    private readonly IConsortiumUserContext _accountUserContext;

    public CompleteHomeTypeCommandHandler(IHomeTypeRepository homeTypeRepository, IConsortiumUserContext accountUserContext, ILogger<CompleteHomeTypeCommandHandler> logger)
        : base(logger)
    {
        _homeTypeRepository = homeTypeRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(CompleteHomeTypeCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeType = await _homeTypeRepository.GetById(
            request.ApplicationId,
            request.HomeTypeId,
            account,
            cancellationToken);

        var errors = PerformWithValidation(() => homeType.CompleteHomeType(request.IsSectionCompleted));
        if (errors.Any())
        {
            return new OperationResult(errors);
        }

        await _homeTypeRepository.Save(homeType, account.SelectedOrganisationId(), cancellationToken);
        return OperationResult.Success();
    }
}
