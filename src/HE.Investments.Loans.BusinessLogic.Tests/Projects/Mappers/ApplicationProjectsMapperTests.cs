using HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using HE.Investments.Loans.Common.Tests.TestData;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.Mappers;
public class ApplicationProjectsMapperTests
{
    [Fact]
    public void StartDateShouldBeEmpty_WhenStartDateExistNotProvided()
    {
        // given
        var dto = LoanApplicationDtoTestBuilder
            .New()
            .WithProject(
                SiteDetailsDtoTestBuilder
                .New()
                .WithStartDate(null))
            .Build();

        // when
        var applicationProjects = ApplicationProjectsMapper.Map(dto);

        // then
        applicationProjects.GetActiveProjects().Should().HaveCount(1);

        var project = applicationProjects.GetActiveProjects().Single();

        project.StartDate.Should().BeNull();
    }

    [Fact]
    public void StartDateShouldNotExist_WhenProjectHasStartDateIsFalse()
    {
        // given
        var dto = LoanApplicationDtoTestBuilder
            .New()
            .WithProject(
                SiteDetailsDtoTestBuilder
                .New()
                .WithStartDate(false))
            .Build();

        // when
        var applicationProjects = ApplicationProjectsMapper.Map(dto);

        // then
        applicationProjects.GetActiveProjects().Should().HaveCount(1);

        var project = applicationProjects.GetActiveProjects().Single();

        project.StartDate.Should().NotBeNull();

        project.StartDate!.Value.HasValue.Should().BeFalse();
    }

    [Fact]
    public void StartDateShouldExistAndHasValue_WhenProjectHasStartDate()
    {
        // given
        var dto = LoanApplicationDtoTestBuilder
            .New()
            .WithProject(
                SiteDetailsDtoTestBuilder
                .New()
                .WithStartDate(true, StartDateTestData.CorrectDate.Value))
            .Build();

        // when
        var applicationProjects = ApplicationProjectsMapper.Map(dto);

        // then
        applicationProjects.GetActiveProjects().Should().HaveCount(1);

        var project = applicationProjects.GetActiveProjects().Single();

        project.StartDate.Should().NotBeNull();

        project.StartDate.Should().Be(StartDateTestData.CorrectDate);
    }
}
