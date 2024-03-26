using FluentAssertions;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Common.Utils;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class IsProjectValidForLoanApplicationTests
{
    [Fact]
    public void ShouldReturnFalse_WhenProjectIsNotValidForLoanApplication()
    {
        // given
        var dateTimeProviderMock = DateTimeProviderBuilder.New().ReturnDate(new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Build();
        DateTimeUtil.SetDateTimeProvider(dateTimeProviderMock);
        var project = ProjectEntityBuilder
            .New()
            .WithSupportActivities(new List<SupportActivityType> { SupportActivityType.AcquiringLand })
            .WithAffordableHomesAmount(AffordableHomesAmount.OnlyOpenMarketHomes)
            .WithOrganisationHomesBuilt(5000)
            .WithIsSiteIdentified(true)
            .WithIsSupportRequired(true)
            .WithRequiredFunding(true, RequiredFundingOption.Between1MlnAnd5Mln)
            .WithIsProfit(true)
            .WithExpectedStartDate("01", "2022")
            .Build();

        // when
        var result = project.IsProjectValidForLoanApplication();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenProjectIsValidForLoanApplication()
    {
        // given
        var dateTimeProviderMock = DateTimeProviderBuilder.New().ReturnDate(new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Build();
        DateTimeUtil.SetDateTimeProvider(dateTimeProviderMock);
        var project = ProjectEntityBuilder
            .New()
            .WithSupportActivities(new List<SupportActivityType> { SupportActivityType.DevelopingHomes })
            .WithAffordableHomesAmount(AffordableHomesAmount.OnlyOpenMarketHomes)
            .WithOrganisationHomesBuilt(1000)
            .WithIsSiteIdentified(true)
            .WithIsSupportRequired(true)
            .WithRequiredFunding(true, RequiredFundingOption.Between1MlnAnd5Mln)
            .WithIsProfit(true)
            .WithExpectedStartDate("01", "2022")
            .Build();

        // when
        var result = project.IsProjectValidForLoanApplication();

        // then
        result.Should().BeTrue();
    }
}
