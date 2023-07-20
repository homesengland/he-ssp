using FluentAssertions;
using HE.InvestmentLoans.BusinessLogic.Application.Repositories;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Application.Mappers;

[TestClass]
public class ApplicationStatusMapperTests
{
    [TestMethod]
    [DataRow(ApplicationStatus.Draft, "draft")]
    [DataRow(ApplicationStatus.Submitted, "submitted")]
    [DataRow(ApplicationStatus.InDueDiligence, "in due diligence")]
    [DataRow(ApplicationStatus.ContractSigned, "contract signed subject to cp")]
    [DataRow(ApplicationStatus.CspSatisfied, "csp satisfied")]
    [DataRow(ApplicationStatus.LoanAvailable, "loan available")]
    [DataRow(ApplicationStatus.HoldRequested, "hold requested")]
    [DataRow(ApplicationStatus.OnHold, "on hold")]
    [DataRow(ApplicationStatus.ReferredBackToApplicant, "referred back to applicant")]
    [DataRow(ApplicationStatus.NA, "n/a")]
    [DataRow(ApplicationStatus.Withdrawn, "withdrawn")]
    [DataRow(ApplicationStatus.NotApproved, "not approved")]
    [DataRow(ApplicationStatus.ApplicationDeclined, "application declined")]
    [DataRow(ApplicationStatus.ApprovedSubjectToContract, "approved subject to contract")]
    public void MapPortalStatus_ToCRMStatus(ApplicationStatus status, string expectedStatus)
    {
        var crmStatus = new ApplicationStatusMapper().MapToCrmStatus(status);

        crmStatus.Should().Be(expectedStatus);
    }

    [TestMethod]
    [DataRow("draft", ApplicationStatus.Draft)]
    [DataRow("submitted", ApplicationStatus.Submitted)]
    [DataRow("in due diligence", ApplicationStatus.InDueDiligence)]
    [DataRow("contract signed subject to cp", ApplicationStatus.ContractSigned)]
    [DataRow("csp satisfied", ApplicationStatus.CspSatisfied)]
    [DataRow("loan available", ApplicationStatus.LoanAvailable)]
    [DataRow("hold requested", ApplicationStatus.HoldRequested)]
    [DataRow("on hold", ApplicationStatus.OnHold)]
    [DataRow("referred back to applicant", ApplicationStatus.ReferredBackToApplicant)]
    [DataRow("n/a", ApplicationStatus.NA)]
    [DataRow("withdrawn", ApplicationStatus.Withdrawn)]
    [DataRow("not approved", ApplicationStatus.NotApproved)]
    [DataRow("application declined", ApplicationStatus.ApplicationDeclined)]
    [DataRow("approved subject to contract", ApplicationStatus.ApprovedSubjectToContract)]
    public void MapCrmStatus_ToPortalStatus(string crmStatus, ApplicationStatus expectedStatus)
    {
        var portalStatus = new ApplicationStatusMapper().MapToPortalStatus(crmStatus);

        portalStatus.Should().Be(expectedStatus);
    }
}
