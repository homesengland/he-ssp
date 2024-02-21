using FluentAssertions;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Tests.Application.Mappers.SectionStatusMapperTests;

public class ToDtoTests
{
    [Theory]
    [InlineData(SectionStatus.NotStarted, (int)invln_AHPSectioncompletionstatusSet.Notstarted)]
    [InlineData(SectionStatus.InProgress, (int)invln_AHPSectioncompletionstatusSet.InProgress)]
    [InlineData(SectionStatus.Completed, (int)invln_AHPSectioncompletionstatusSet.Completed)]
    public void ShouldReturnExpectedValue_WhenSectionStatusIsProvided(SectionStatus status, int expectedValue)
    {
        var result = SectionStatusMapper.ToDto(status);

        result.Should().Be(expectedValue);
    }
}
