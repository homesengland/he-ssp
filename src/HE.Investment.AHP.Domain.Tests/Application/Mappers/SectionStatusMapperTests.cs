using FluentAssertions;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Tests.Application.Mappers;

public class SectionStatusMapperTests
{
    [Theory]
    [InlineData(SectionStatus.NotStarted, (int)invln_ahpsectioncompletionstatusset.Notstarted)]
    [InlineData(SectionStatus.InProgress, (int)invln_ahpsectioncompletionstatusset.InProgress)]
    [InlineData(SectionStatus.Completed, (int)invln_ahpsectioncompletionstatusset.Completed)]
    public void ToDto_GivenSectionStatus_ReturnsExpectedValue(SectionStatus status, int expectedValue)
    {
        var result = SectionStatusMapper.ToDto(status);

        result.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData((int)invln_ahpsectioncompletionstatusset.Notstarted, SectionStatus.NotStarted)]
    [InlineData((int)invln_ahpsectioncompletionstatusset.InProgress, SectionStatus.InProgress)]
    [InlineData((int)invln_ahpsectioncompletionstatusset.Completed, SectionStatus.Completed)]
    public void ToDomain_GivenValue_ReturnsExpectedSectionStatus(int value, SectionStatus expectedStatus)
    {
        var result = SectionStatusMapper.ToDomain(value);

        result.Should().Be(expectedStatus);
    }

    [Fact]
    public void ToDomain_GivenNullValue_ReturnsNotStarted()
    {
        var result = SectionStatusMapper.ToDomain(null);

        result.Should().Be(SectionStatus.NotStarted);
    }

    [Fact]
    public void ToDomain_GivenApplicationStatusWithdrawn_ReturnsWithdrawn()
    {
        var result = SectionStatusMapper.ToDomain(null, ApplicationStatus.Withdrawn);

        result.Should().Be(SectionStatus.Withdrawn);
    }
}
