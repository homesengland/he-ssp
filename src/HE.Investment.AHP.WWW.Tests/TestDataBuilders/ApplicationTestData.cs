using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.WWW.Tests.TestDataBuilders;

public static class ApplicationTestData
{
    public static readonly ApplicationDetails SampleApplication = new(
        new FrontDoorProjectId("project-1234"),
        new AhpApplicationId("test-1234"),
        "appName",
        Tenure.AffordableRent,
        ApplicationStatus.Draft,
        [AhpApplicationOperation.Modification]);
}
