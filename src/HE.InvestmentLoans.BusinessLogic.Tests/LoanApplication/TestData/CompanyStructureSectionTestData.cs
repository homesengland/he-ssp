using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
internal static class CompanyStructureSectionTestData
{
    public static readonly LoanApplicationSection CompletedSection = new(SectionStatus.Completed);
}
