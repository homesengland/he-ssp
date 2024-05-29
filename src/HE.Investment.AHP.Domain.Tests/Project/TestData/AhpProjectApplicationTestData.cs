using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Tests.Project.TestData;

public static class AhpProjectApplicationTestData
{
    public static readonly AhpProjectApplication FirstAhpProjectApplication = new(
        new AhpApplicationId("application-id-1"),
        new ApplicationName("First application"),
        ApplicationStatus.ApplicationSubmitted,
        new SchemeFunding("100000", "10"),
        Tenure.AffordableRent,
        null);

    public static readonly AhpProjectApplication SecondAhpProjectApplication = new(
        new AhpApplicationId("application-id-2"),
        new ApplicationName("Second application"),
        ApplicationStatus.UnderReview,
        new SchemeFunding("99999", "527"),
        Tenure.OlderPersonsSharedOwnership,
        null);

    public static readonly AhpProjectApplication ThirdAhpProjectApplication = new(
        new AhpApplicationId("application-id-3"),
        new ApplicationName("Third application"),
        ApplicationStatus.LoanAvailable,
        new SchemeFunding("368434", "345"),
        Tenure.RentToBuy,
        null);
}
