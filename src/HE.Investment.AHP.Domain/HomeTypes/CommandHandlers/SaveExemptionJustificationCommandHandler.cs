using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveExemptionJustificationCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveExemptionJustificationCommand>
{
    public SaveExemptionJustificationCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveExemptionJustificationCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<SaveExemptionJustificationCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveExemptionJustificationCommand request, IHomeTypeEntity homeType) =>
        {
            var exemptionJustification = request.ExemptionJustification.IsNotProvided()
                ? null
                : new MoreInformation(request.ExemptionJustification!, "The exemption justification");
            homeType.TenureDetails.ChangeExemptionJustification(exemptionJustification);
        },
    };
}
