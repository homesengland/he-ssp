using FluentAssertions;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Mappers;

namespace HE.Investment.AHP.Domain.Tests.Application.Mappers;

public class AhpApplicationStatusMapperTests
{
    [Theory]
    [InlineData(ApplicationStatus.New, 858110000)]
    [InlineData(ApplicationStatus.Draft, 858110001)]
    [InlineData(ApplicationStatus.ApplicationSubmitted, 858110002)]
    [InlineData(ApplicationStatus.Deleted, 858110003)]
    [InlineData(ApplicationStatus.OnHold, 858110004)]
    [InlineData(ApplicationStatus.Withdrawn, 858110005)]
    [InlineData(ApplicationStatus.UnderReview, 858110006)]
    [InlineData(ApplicationStatus.Rejected, 858110007)]
    [InlineData(ApplicationStatus.RequestedEditing, 858110008)]
    [InlineData(ApplicationStatus.ReferredBackToApplicant, 858110009)]
    [InlineData(ApplicationStatus.ApprovedSubjectToContract, 858110010)]
    [InlineData(ApplicationStatus.Approved, 858110011)]
    public void MapPortalStatus_ToCRMStatus(ApplicationStatus status, int expectedStatus)
    {
        var crmStatus = AhpApplicationStatusMapper.MapToCrmStatus(status);

        crmStatus.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData(858110000, ApplicationStatus.New)]
    [InlineData(858110001, ApplicationStatus.Draft)]
    [InlineData(858110002, ApplicationStatus.ApplicationSubmitted)]
    [InlineData(858110003, ApplicationStatus.Deleted)]
    [InlineData(858110004, ApplicationStatus.OnHold)]
    [InlineData(858110005, ApplicationStatus.Withdrawn)]
    [InlineData(858110006, ApplicationStatus.UnderReview)]
    [InlineData(858110007, ApplicationStatus.Rejected)]
    [InlineData(858110008, ApplicationStatus.RequestedEditing)]
    [InlineData(858110009, ApplicationStatus.ReferredBackToApplicant)]
    [InlineData(858110010, ApplicationStatus.ApprovedSubjectToContract)]
    [InlineData(858110011, ApplicationStatus.Approved)]
    public void MapCrmStatus_ToPortalStatus(int crmStatus, ApplicationStatus expectedStatus)
    {
        var portalStatus = AhpApplicationStatusMapper.MapToPortalStatus(crmStatus);

        portalStatus.Should().Be(expectedStatus);
    }
}
