namespace HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.Allocation;

public class PhaseData
{
    public string PhaseId { get; private set; }

    public string PhaseName { get; private set; }

    public void SetPhaseName(string phaseName)
    {
        PhaseName = phaseName;
    }

    public void SetPhaseId(string phaseId)
    {
        PhaseId = phaseId;
    }
}
