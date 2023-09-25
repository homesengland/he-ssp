using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Application.Mappers;

[TestClass]
public class ApplicationStatusMapperTests
{
    [TestMethod]
    [DataRow(ApplicationStatus.Draft, 858110000)]
    [DataRow(ApplicationStatus.ApplicationSubmitted, 858110001)]
    [DataRow(ApplicationStatus.InDueDiligence, 858110013)]
    [DataRow(ApplicationStatus.ContractSigned, 858110015)]
    [DataRow(ApplicationStatus.CspSatisfied, 858110016)]
    [DataRow(ApplicationStatus.LoanAvailable, 858110017)]
    [DataRow(ApplicationStatus.HoldRequested, 858110004)]
    [DataRow(ApplicationStatus.OnHold, 858110005)]
    [DataRow(ApplicationStatus.ReferredBackToApplicant, 858110008)]
    [DataRow(ApplicationStatus.NA, 858110019)]
    [DataRow(ApplicationStatus.Withdrawn, 858110003)]
    [DataRow(ApplicationStatus.ApplicationDeclined, 858110012)]
    [DataRow(ApplicationStatus.ApprovedSubjectToContract, 858110014)]
    [DataRow(ApplicationStatus.UnderReview, 858110009)]
    [DataRow(ApplicationStatus.ApplicationUnderReview, 858110002)]
    [DataRow(ApplicationStatus.CashflowRequested, 858110006)]
    [DataRow(ApplicationStatus.CashflowUnderReview, 858110007)]
    [DataRow(ApplicationStatus.SentForApproval, 858110010)]
    [DataRow(ApplicationStatus.ApprovedSubjectToDueDiligence, 858110011)]
    [DataRow(ApplicationStatus.New, 858110018)]
    public void MapPortalStatus_ToCRMStatus(ApplicationStatus status, int expectedStatus)
    {
        var crmStatus = ApplicationStatusMapper.MapToCrmStatus(status);

        crmStatus.Should().Be(expectedStatus);
    }

    [TestMethod]
    [DataRow(858110000, ApplicationStatus.Draft)]
    [DataRow(858110001, ApplicationStatus.ApplicationSubmitted)]
    [DataRow(858110013, ApplicationStatus.InDueDiligence)]
    [DataRow(858110015, ApplicationStatus.ContractSigned)]
    [DataRow(858110016, ApplicationStatus.CspSatisfied)]
    [DataRow(858110017, ApplicationStatus.LoanAvailable)]
    [DataRow(858110004, ApplicationStatus.HoldRequested)]
    [DataRow(858110005, ApplicationStatus.OnHold)]
    [DataRow(858110008, ApplicationStatus.ReferredBackToApplicant)]
    [DataRow(858110019, ApplicationStatus.NA)]
    [DataRow(858110003, ApplicationStatus.Withdrawn)]
    [DataRow(858110012, ApplicationStatus.ApplicationDeclined)]
    [DataRow(858110014, ApplicationStatus.ApprovedSubjectToContract)]
    [DataRow(858110009, ApplicationStatus.UnderReview)]
    [DataRow(858110002, ApplicationStatus.ApplicationUnderReview)]
    [DataRow(858110006, ApplicationStatus.CashflowRequested)]
    [DataRow(858110007, ApplicationStatus.CashflowUnderReview)]
    [DataRow(858110010, ApplicationStatus.SentForApproval)]
    [DataRow(858110011, ApplicationStatus.ApprovedSubjectToDueDiligence)]
    [DataRow(858110018, ApplicationStatus.New)]
    public void MapCrmStatus_ToPortalStatus(int crmStatus, ApplicationStatus expectedStatus)
    {
        var portalStatus = ApplicationStatusMapper.MapToPortalStatus(crmStatus);

        portalStatus.Should().Be(expectedStatus);
    }
}
