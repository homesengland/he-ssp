using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.Programme.Domain.Crm;
using HE.Investments.Programme.Domain.Entities;
using HE.Investments.Programme.Domain.ValueObjects;

namespace HE.Investments.Programme.Domain.Repositories;

public class ProgrammeRepository : IProgrammeRepository
{
    private readonly IProgrammeCrmContext _crmContext;

    public ProgrammeRepository(IProgrammeCrmContext crmContext)
    {
        _crmContext = crmContext;
    }

    public async Task<ProgrammeEntity> GetProgramme(ProgrammeId programmeId, CancellationToken cancellationToken)
    {
        var programme = await _crmContext.GetProgramme(programmeId.ToGuidAsString(), cancellationToken);

        return MapProgramme(programme);
    }

    public async Task<IList<ProgrammeEntity>> GetProgrammes(ProgrammeType programmeType, CancellationToken cancellationToken)
    {
        var programmes = await _crmContext.GetProgrammes(cancellationToken);

        return programmes.Select(MapProgramme).Where(x => x.ProgrammeType == programmeType).ToList();
    }

    private static ProgrammeEntity MapProgramme(ProgrammeDto programme)
    {
        var programmeId = ProgrammeId.From(programme.Id);
        return new ProgrammeEntity(
            programmeId,
            programme.name,
            MapProgrammeDates(programmeId, programme.startOn, programme.endOn, true),
            MapProgrammeDates(programmeId, programme.assignFundingStartDate, programme.assignFundingEndDate),
            MapProgrammeDates(programmeId, programme.startOnSiteStartDate, programme.startOnSiteEndDate),
            MapProgrammeDates(programmeId, programme.completionStartDate, programme.completionEndDate));
    }

    private static ProgrammeDates MapProgrammeDates(ProgrammeId programmeId, DateTime? start, DateTime? end, bool isRequired = false)
    {
        if (isRequired && (start.IsNotProvided() || end.IsNotProvided()))
        {
            throw new InvalidOperationException($"Programme {programmeId} does not have dates set.");
        }

        return new ProgrammeDates(
            start.HasValue ? DateOnly.FromDateTime(start.Value) : null,
            end.HasValue ? DateOnly.FromDateTime(end.Value) : null);
    }
}
