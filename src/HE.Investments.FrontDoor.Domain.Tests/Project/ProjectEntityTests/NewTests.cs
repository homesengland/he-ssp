using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using Moq;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class NewTests
{
    [Fact]
    public async Task ShouldThrowException_WhenProjectWithTheSameNameAlreadyExists()
    {
        // given
        var projectName = new ProjectName("existing name");
        var projectNameExists = MockProjectNameExists(true);

        // when
        var create = () => ProjectEntity.New(projectName, projectNameExists, CancellationToken.None);

        // then
        var errors = (await create.Should().ThrowAsync<DomainValidationException>()).Which.OperationResult.Errors;

        errors.Should().HaveCount(1);
        errors[0].AffectedField.Should().Be("Name");
    }

    [Fact]
    public async Task ShouldReturnNewEntity_WhenProjectWithTheSameNameDoesNotExist()
    {
        // given
        var projectName = new ProjectName("unique name");
        var projectNameExists = MockProjectNameExists(false);

        // when
        var result = await ProjectEntity.New(projectName, projectNameExists, CancellationToken.None);

        // then
        result.Id.Value.Should().BeEmpty();
        result.Name.Value.Should().Be("unique name");
        result.IsEnglandHousingDelivery.Should().BeTrue();
    }

    private static IProjectNameExists MockProjectNameExists(bool exists)
    {
        var mock = new Mock<IProjectNameExists>();
        mock.Setup(x => x.DoesExist(It.IsAny<ProjectName>(), It.IsAny<FrontDoorProjectId?>(), CancellationToken.None))
            .ReturnsAsync(exists);

        return mock.Object;
    }
}
