using FluentAssertions;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Application.Mappers;

[TestClass]
public class ApplicationStatusMapperTests
{
    [TestMethod]
    [DataRow(ApplicationStatus.Draft, 858110000)]
    [DataRow(ApplicationStatus.Submitted, 858110001)]
    [DataRow(ApplicationStatus.InDueDiligence, 858110003)]
    [DataRow(ApplicationStatus.ContractSigned, 858110004)]
    [DataRow(ApplicationStatus.CspSatisfied, 858110006)]
    [DataRow(ApplicationStatus.LoanAvailable, 858110007)]
    [DataRow(ApplicationStatus.HoldRequested, 858110008)]
    [DataRow(ApplicationStatus.OnHold, 858110009)]
    [DataRow(ApplicationStatus.ReferredBackToApplicant, 858110010)]
    [DataRow(ApplicationStatus.NA, 858110011)]
    [DataRow(ApplicationStatus.Withdrawn, 858110012)]
    [DataRow(ApplicationStatus.NotApproved, 858110013)]
    [DataRow(ApplicationStatus.ApplicationDeclined, 858110014)]
    [DataRow(ApplicationStatus.ApprovedSubjectToContract, 858110015)]
    [DataRow(ApplicationStatus.UnderReview, 858110002)]
    public void MapPortalStatus_ToCRMStatus(ApplicationStatus status, int expectedStatus)
    {
        var crmStatus = ApplicationStatusMapper.MapToCrmStatus(status);

        crmStatus.Should().Be(expectedStatus);
    }

    [TestMethod]
    [DataRow(858110000, ApplicationStatus.Draft)]
    [DataRow(858110001, ApplicationStatus.Submitted)]
    [DataRow(858110003, ApplicationStatus.InDueDiligence)]
    [DataRow(858110004, ApplicationStatus.ContractSigned)]
    [DataRow(858110006, ApplicationStatus.CspSatisfied)]
    [DataRow(858110007, ApplicationStatus.LoanAvailable)]
    [DataRow(858110008, ApplicationStatus.HoldRequested)]
    [DataRow(858110009, ApplicationStatus.OnHold)]
    [DataRow(858110010, ApplicationStatus.ReferredBackToApplicant)]
    [DataRow(858110011, ApplicationStatus.NA)]
    [DataRow(858110012, ApplicationStatus.Withdrawn)]
    [DataRow(858110013, ApplicationStatus.NotApproved)]
    [DataRow(858110014, ApplicationStatus.ApplicationDeclined)]
    [DataRow(858110015, ApplicationStatus.ApprovedSubjectToContract)]
    [DataRow(858110002, ApplicationStatus.UnderReview)]
    public void MapCrmStatus_ToPortalStatus(int crmStatus, ApplicationStatus expectedStatus)
    {
        var portalStatus = ApplicationStatusMapper.MapToPortalStatus(crmStatus);

        portalStatus.Should().Be(expectedStatus);
    }
}
