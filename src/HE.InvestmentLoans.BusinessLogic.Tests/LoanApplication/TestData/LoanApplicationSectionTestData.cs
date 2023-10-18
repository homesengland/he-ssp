using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
internal static class LoanApplicationSectionTestData
{
    public static readonly LoanApplicationSection CompletedSection = new(SectionStatus.Completed);

    public static readonly LoanApplicationSection IncompletedSection = new(SectionStatus.InProgress);
}
