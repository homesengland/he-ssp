using FluentAssertions;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using Moq;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class ProvideNameTests
{
    [Fact]
    public async Task ShouldThrowException_WhenProjectWithTheSameNameAlreadyExists()
    {
        // given
        var project = ProjectEntityBuilder.New().Build();
        var projectName = new ProjectName("existing name");
        var projectNameExists = MockProjectNameExists(true);

        // when
        var provideName = () => project.ProvideName(projectName, projectNameExists, CancellationToken.None);

        // then
        var errors = (await provideName.Should().ThrowAsync<DomainValidationException>()).Which.OperationResult.Errors;

        errors.Should().HaveCount(1);
        errors[0].AffectedField.Should().Be("Name");
    }

    [Fact]
    public async Task ShouldChangeProjectName_WhenProjectWithTheSameNameDoesNotExist()
    {
        // given
        var project = ProjectEntityBuilder.New().Build();
        var projectName = new ProjectName("unique name");
        var projectNameExists = MockProjectNameExists(false);

        // when
        await project.ProvideName(projectName, projectNameExists, CancellationToken.None);

        // then
        project.Name.Value.Should().Be("unique name");
    }

    private static IProjectNameExists MockProjectNameExists(bool exists)
    {
        var mock = new Mock<IProjectNameExists>();
        mock.Setup(x => x.DoesExist(It.IsAny<ProjectName>(), CancellationToken.None))
            .ReturnsAsync(exists);

        return mock.Object;
    }
}
