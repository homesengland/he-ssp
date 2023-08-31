using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Application.Mappers;

[TestClass]
public class ApplicationStatusMapperTests
{
    [TestMethod]
    [DataRow(ApplicationStatus.Draft, 858110000)]
    [DataRow(ApplicationStatus.Submitted, 858110001)]
    [DataRow(ApplicationStatus.InDueDiligence, 858110013)]
    [DataRow(ApplicationStatus.ContractSigned, 858110015)]
    [DataRow(ApplicationStatus.CspSatisfied, 858110016)]
    [DataRow(ApplicationStatus.LoanAvailable, 858110017)]
    [DataRow(ApplicationStatus.HoldRequested, 858110004)]
    [DataRow(ApplicationStatus.OnHold, 858110005)]
    [DataRow(ApplicationStatus.ReferredBackToApplicant, 858110008)]
    [DataRow(ApplicationStatus.NA, 858110019)]
    [DataRow(ApplicationStatus.Withdrawn, 858110003)]
    [DataRow(ApplicationStatus.NotApproved, 858110010)]
    [DataRow(ApplicationStatus.ApplicationDeclined, 858110012)]
    [DataRow(ApplicationStatus.ApprovedSubjectToContract, 858110014)]
    [DataRow(ApplicationStatus.UnderReview, 858110009)]
    public void MapPortalStatus_ToCRMStatus(ApplicationStatus status, int expectedStatus)
    {
        var crmStatus = ApplicationStatusMapper.MapToCrmStatus(status);

        crmStatus.Should().Be(expectedStatus);
    }

    [TestMethod]
    [DataRow(858110000, ApplicationStatus.Draft)]
    [DataRow(858110001, ApplicationStatus.Submitted)]
    [DataRow(858110013, ApplicationStatus.InDueDiligence)]
    [DataRow(858110015, ApplicationStatus.ContractSigned)]
    [DataRow(858110016, ApplicationStatus.CspSatisfied)]
    [DataRow(858110017, ApplicationStatus.LoanAvailable)]
    [DataRow(858110004, ApplicationStatus.HoldRequested)]
    [DataRow(858110005, ApplicationStatus.OnHold)]
    [DataRow(858110008, ApplicationStatus.ReferredBackToApplicant)]
    [DataRow(858110019, ApplicationStatus.NA)]
    [DataRow(858110003, ApplicationStatus.Withdrawn)]
    [DataRow(858110010, ApplicationStatus.NotApproved)]
    [DataRow(858110012, ApplicationStatus.ApplicationDeclined)]
    [DataRow(858110014, ApplicationStatus.ApprovedSubjectToContract)]
    [DataRow(858110009, ApplicationStatus.UnderReview)]
    public void MapCrmStatus_ToPortalStatus(int crmStatus, ApplicationStatus expectedStatus)
    {
        var portalStatus = ApplicationStatusMapper.MapToPortalStatus(crmStatus);

        portalStatus.Should().Be(expectedStatus);
    }
}
