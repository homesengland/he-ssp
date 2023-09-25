using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Tests;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;

public class LoanApplicationTestBuilder : TestEntityBuilderBase<LoanApplicationEntity>
{
    public LoanApplicationTestBuilder(LoanApplicationEntity item)
    {
        Item = item;
        Item.LegacyModel = new LoanApplicationViewModel { ReferenceNumber = ReferenceNumberTestData.One, };
    }

    public static LoanApplicationTestBuilder NewDraft(UserAccount userAccount) => new(
        new LoanApplicationEntity(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            userAccount,
            ApplicationStatus.Draft,
            DateTimeTestData.SeptemberDay20Year2023At0736,
            FundingPurpose.BuildingNewHomes));
}
