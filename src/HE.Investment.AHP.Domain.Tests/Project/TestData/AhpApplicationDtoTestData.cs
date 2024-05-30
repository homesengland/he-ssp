using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Tests.Project.TestData;

public static class AhpApplicationDtoTestData
{
    public static readonly AhpApplicationDto FirstAhpApplication = new()
    {
        id = "ahp-application-id-1",
        name = "First application",
        applicationStatus = (int)invln_ExternalStatus.ApplicationSubmitted,
        fundingRequested = 100000,
        noOfHomes = 10,
        tenure = (int)invln_Tenure.Affordablerent,
        lastExternalModificationOn = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
    };

    public static readonly AhpApplicationDto SecondAhpApplication = new()
    {
        id = "ahp-application-id-2",
        name = "Second application",
        applicationStatus = (int)invln_ExternalStatus.ApplicationSubmitted,
        fundingRequested = 200000,
        noOfHomes = 20,
        tenure = (int)invln_Tenure.Socialrent,
        lastExternalModificationOn = new DateTime(2021, 2, 2, 0, 0, 0, DateTimeKind.Utc),
    };

    public static readonly AhpApplicationDto ThirdAhpApplication = new()
    {
        id = "ahp-application-id-3",
        name = "Third application",
        applicationStatus = (int)invln_ExternalStatus.ApplicationSubmitted,
        fundingRequested = 300000,
        noOfHomes = 30,
        tenure = (int)invln_Tenure.Sharedownership,
        lastExternalModificationOn = new DateTime(2021, 3, 3, 0, 0, 0, DateTimeKind.Utc),
    };

    public static readonly AhpApplicationDto FourthAhpApplication = new()
    {
        id = "ahp-application-id-4",
        name = "Fourth application",
        applicationStatus = (int)invln_ExternalStatus.ApplicationSubmitted,
        fundingRequested = 400000,
        noOfHomes = 40,
        tenure = (int)invln_Tenure.Renttobuy,
        lastExternalModificationOn = new DateTime(2021, 4, 4, 0, 0, 0, DateTimeKind.Utc),
    };

    public static readonly AhpApplicationDto FifthAhpApplication = new()
    {
        id = "ahp-application-id-5",
        name = "Fifth application",
        applicationStatus = (int)invln_ExternalStatus.ApplicationSubmitted,
        fundingRequested = 500000,
        noOfHomes = 50,
        tenure = (int)invln_Tenure.HOLD,
        lastExternalModificationOn = new DateTime(2021, 5, 5, 0, 0, 0, DateTimeKind.Utc),
    };
}
