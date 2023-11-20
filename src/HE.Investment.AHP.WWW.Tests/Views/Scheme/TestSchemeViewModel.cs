using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Http;

namespace HE.Investment.AHP.WWW.Tests.Views.Scheme;

public static class TestSchemeViewModel
{
    public static SchemeViewModel Test =>
        new(
            "A1",
            "App Name",
            "345.45",
            "100",
            "test affordability evidence",
            "some sales risk",
            "type and tenure justification",
            "scheme and proposal justification",
            "stakeholders accepted everything",
            new List<FileModel>(),
            new List<IFormFile>());
}
