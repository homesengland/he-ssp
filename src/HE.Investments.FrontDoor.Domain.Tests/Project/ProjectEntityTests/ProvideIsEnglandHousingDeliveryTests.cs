using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class ProvideIsEnglandHousingDeliveryTests
{
    [Fact]
    public void ShouldThrowException_WhenFlagIsNotProvided()
    {
        // given
        var project = ProjectEntityBuilder.New().Build();

        // when
        var provide = () => project.ProvideIsEnglandHousingDelivery(null);

        // then
        var errors = provide.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;

        errors.Should().HaveCount(1);
        errors[0].AffectedField.Should().Be("isEnglandHousingDelivery");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldChangeProjectName_WhenProjectWithTheSameNameDoesNotExist(bool isEnglandHousingDelivery)
    {
        // given
        var project = ProjectEntityBuilder.New().Build();

        // when
        project.ProvideIsEnglandHousingDelivery(isEnglandHousingDelivery);

        // then
        project.IsEnglandHousingDelivery.Should().Be(isEnglandHousingDelivery);
    }
}
