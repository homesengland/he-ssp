using HE.Investment.AHP.WWW.Models.Scheme;

namespace HE.Investment.AHP.WWW.Tests.Views.Scheme;

public static class TestSchemeViewModel
{
    public static SchemeViewModel Test =>
        new(
            "A1",
            "App Name",
            "S1",
            345.45m,
            100,
            "test affordability evidence",
            "some sales risk");
}
