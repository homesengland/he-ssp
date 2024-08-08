using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestData;

public static class AhpProjectApplicationTestData
{
    public static readonly AhpProjectApplication FirstAhpProjectApplication = new(
        new AhpApplicationId("application-id-1"),
        "First application",
        ApplicationStatus.ApplicationSubmitted,
        100000,
        10,
        Tenure.AffordableRent,
        null,
        "Oxford");

    public static readonly AhpProjectApplication SecondAhpProjectApplication = new(
        new AhpApplicationId("application-id-2"),
        "Second application",
        ApplicationStatus.UnderReview,
        99999,
        527,
        Tenure.OlderPersonsSharedOwnership,
        null,
        "Liverpool");

    public static readonly AhpProjectApplication ThirdAhpProjectApplication = new(
        new AhpApplicationId("application-id-3"),
        "Third application",
        ApplicationStatus.LoanAvailable,
        368434,
        345,
        Tenure.RentToBuy,
        null,
        "London");
}
