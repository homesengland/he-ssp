using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Tests.TestDataBuilders;

public static class ApplicationTestData
{
    public static readonly ApplicationDetails SampleApplication = new(
        new AhpApplicationId("test-1234"),
        "appName",
        Tenure.AffordableRent,
        ApplicationStatus.Draft,
        new[] { AhpApplicationOperation.Modification });
}
