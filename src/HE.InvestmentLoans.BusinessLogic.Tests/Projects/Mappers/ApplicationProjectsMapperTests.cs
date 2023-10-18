using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.Common.Tests.TestData;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.Mappers;
public class ApplicationProjectsMapperTests
{
    private readonly DateTime _now = DateTimeTestData.SeptemberDay20Year2023At0736;

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
        var applicationProjects = ApplicationProjectsMapper.Map(dto, _now);

        // then
        applicationProjects.ActiveProjects.Should().HaveCount(1);

        var project = applicationProjects.ActiveProjects.Single();

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
        var applicationProjects = ApplicationProjectsMapper.Map(dto, _now);

        // then
        applicationProjects.ActiveProjects.Should().HaveCount(1);

        var project = applicationProjects.ActiveProjects.Single();

        project.StartDate.Should().NotBeNull();

        project.StartDate!.Exists.Should().BeFalse();
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
        var applicationProjects = ApplicationProjectsMapper.Map(dto, _now);

        // then
        applicationProjects.ActiveProjects.Should().HaveCount(1);

        var project = applicationProjects.ActiveProjects.Single();

        project.StartDate.Should().NotBeNull();

        project.StartDate.Should().Be(StartDateTestData.CorrectDate);
    }
}
