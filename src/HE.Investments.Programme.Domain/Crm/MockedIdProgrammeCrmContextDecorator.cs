using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Programme.Domain.Crm;

// TODO: AB#90826 Remove this class when ProgrammeId will be returned from CRM
public class MockedIdProgrammeCrmContextDecorator : IProgrammeCrmContext
{
    private readonly IProgrammeCrmContext _decorated;

    public MockedIdProgrammeCrmContextDecorator(IProgrammeCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<ProgrammeDto> GetProgramme(string programmeId, CancellationToken cancellationToken)
    {
        var programme = await _decorated.GetProgramme(programmeId, cancellationToken);
        programme.id = programmeId;

        return programme;
    }

    public async Task<IList<ProgrammeDto>> GetProgrammes(CancellationToken cancellationToken)
    {
        var programmes = await _decorated.GetProgrammes(cancellationToken);
        foreach (var programme in programmes)
        {
            programme.id = "d5fe3baa-eeae-ee11-a569-0022480041cf";
        }

        return programmes;
    }
}
