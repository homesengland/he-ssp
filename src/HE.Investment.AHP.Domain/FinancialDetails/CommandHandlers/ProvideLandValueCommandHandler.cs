﻿using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;

public class ProvideLandValueCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideLandValueCommand, OperationResult>
{
    public ProvideLandValueCommandHandler(
        IFinancialDetailsRepository repository,
        IConsortiumUserContext accountUserContext,
        ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, accountUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideLandValueCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails =>
            {
                var landValue = new LandValue(
                    request.LandValue.IsProvided() ? new CurrentLandValue(request.LandValue!) : null,
                    request.LandOwnership);

                financialDetails.ProvideLandValue(landValue);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
