using System.ComponentModel;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.WWW.Models.UserOrganisation;

namespace HE.Investments.Account.WWW.Controllers.Consts;

public static class ProgrammesConsts
{
    private static readonly IDictionary<ProgrammeType, ProgrammeModel> Programmes = new Dictionary<ProgrammeType, ProgrammeModel>
    {
        {
            ProgrammeType.Loans, new(
                "Levelling up Home Building Fund",
                "You can start a new Levelling Up Home Building Fund application here. This will not affect any of your previous applications.",
                "TODO",
                "TODO",
                "TODO")
        },
        {
            ProgrammeType.Ahp, new(
                "Affordable Homes Programme 2021-2026 Continuous Market Engagement",
                "You can start a new Affordable Homes Programme application here. This will not affect any of your previous applications.",
                "TODO",
                "TODO",
                "TODO")
        },
    };

    public static ProgrammeModel GetByType(ProgrammeType type)
    {
        return Programmes[type] ?? throw new InvalidEnumArgumentException($"Programme for {type} does not exist.");
    }
}
