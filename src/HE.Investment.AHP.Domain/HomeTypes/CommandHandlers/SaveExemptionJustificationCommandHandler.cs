using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveExemptionJustificationCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveExemptionJustificationCommand>
{
    public SaveExemptionJustificationCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveExemptionJustificationCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.TenureDetails];

    protected override IEnumerable<Action<SaveExemptionJustificationCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveExemptionJustificationCommand request, IHomeTypeEntity homeType) =>
        {
            var exemptionJustification = request.ExemptionJustification.IsNotProvided()
                ? null
                : new MoreInformation(request.ExemptionJustification!, "exemption justification");
            homeType.TenureDetails.ChangeExemptionJustification(exemptionJustification);
        },
    ];
}
