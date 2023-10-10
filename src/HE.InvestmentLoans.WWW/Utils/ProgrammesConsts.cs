using HE.InvestmentLoans.WWW.Controllers;
using HE.InvestmentLoans.WWW.Models.UserOrganisation;

namespace HE.InvestmentLoans.WWW.Utils;

public static class ProgrammesConsts
{
    public static readonly ProgrammeModel LoansProgramme = new(
        "Levelling up Home Building Fund",
        "You can start a new Levelling Up Home Building Fund application here. This will not affect any of your previous applications.",
        nameof(LoanApplicationV2Controller.StartApplication),
        nameof(LoanApplicationV2Controller.ApplicationDashboard),
        "LoanApplicationV2");

    // TODO: fix params for AHP program
    public static readonly ProgrammeModel AhpProgramme = new(
        "Affordable Homes Programme 2021-2026 Continuous Market Engagement",
        "You can start a new Affordable Homes Programme application here. This will not affect any of your previous applications.",
        "TODO",
        "TODO",
        "TODO");
}
