using HE.Investments.Loans.WWW.Controllers;
using HE.Investments.Loans.WWW.Models.UserOrganisation;

namespace HE.Investments.Loans.WWW.Utils;

public static class ProgrammesConsts
{
    public static readonly ProgrammeModel LoansProgramme = new(
        "Levelling up Home Building Fund",
        "Start a new Levelling Up Home Building Fund application. This will not affect any of your previous applications.",
        nameof(LoanApplicationV2Controller.AboutLoan),
        nameof(LoanApplicationV2Controller.ApplicationDashboard),
        "LoanApplicationV2");

    //// TODO: fix params for AHP program
    public static readonly ProgrammeModel AhpProgramme = new(
        "Affordable Homes Programme 2021-2026 Continuous Market Engagement",
        "Start a new Affordable Homes Programme application. This will not affect any of your previous applications.",
        "TODO",
        "TODO",
        "TODO");
}
