using System.ComponentModel;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.WWW.Models.UserOrganisation;

namespace HE.Investments.Account.WWW.Routing;

public class Programmes : IProgrammes
{
    private readonly ProgrammeUrlConfig _programmeUrlConfig;

    private readonly IDictionary<ProgrammeType, ProgrammeModel> _programmes;

    public Programmes(ProgrammeUrlConfig programmeUrlConfig)
    {
        _programmeUrlConfig = programmeUrlConfig;
        _programmes = new Dictionary<ProgrammeType, ProgrammeModel>
        {
            {
                ProgrammeType.Loans, new(
                    "Levelling up Home Building Fund",
                    "You can start a new Levelling Up Home Building Fund application here. This will not affect any of your previous applications.",
                    $"{_programmeUrlConfig.Loans}/application",
                    $"{_programmeUrlConfig.Loans}/dashboard")
            },
            {
                ProgrammeType.Ahp, new(
                    "Affordable Homes Programme 2021-2026 Continuous Market Engagement",
                    "You can start a new Affordable Homes Programme application here. This will not affect any of your previous applications.",
                    $"{_programmeUrlConfig.Ahp}/application/name",
                    $"{_programmeUrlConfig.Ahp}/application")
            },
        };
    }

    public ProgrammeModel GetProgramme(ProgrammeType programmeType)
    {
        return _programmes[programmeType] ?? throw new InvalidEnumArgumentException($"Programme for {programmeType} does not exist.");
    }

    public string GetApplicationUrl(ProgrammeType programmeType, string applicationId)
    {
        if (programmeType == ProgrammeType.Ahp)
        {
            return $"{_programmeUrlConfig.Ahp}/application/{applicationId}";
        }

        if (programmeType == ProgrammeType.Loans)
        {
            return $"{_programmeUrlConfig.Loans}/application/{applicationId}";
        }

        throw new InvalidEnumArgumentException($"Programme for {programmeType} does not exist.");
    }
}
