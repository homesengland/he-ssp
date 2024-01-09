using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideTypeOfHomesCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideTypeOfHomesCommand>
{
    public ProvideTypeOfHomesCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideTypeOfHomesCommand request)
    {
        if (request.TypeOfHomes.IsProvided())
        {
            entity.ProvideTypeOfHomes(request.TypeOfHomes!.Value);
        }

        return Task.FromResult(OperationResult.Success());
    }
}
