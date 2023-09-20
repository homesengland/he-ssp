using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.Funding.Commands;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.CommandHandlers;
public class CheckAnswersFundingSectionCommandHandlerTests : TestBase<CheckAnswersFundingSectionCommandHandler>
{
    [Fact]
    public async Task ShouldSaveFundingAsCompleted()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var fundingEntity = FundingEntityTestBuilder
            .New()
            .WithGrossDevelopmentValue()
            .WithEstimatedTotalCosts()
            .WithAbnormalCosts()
            .WithPrivateSectorFunding()
            .WithRepaymentSystem()
            .WithAdditionalProjects()
            .Build();

        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        FundingRepositoryTestBuilder
            .New()
            .ReturnFundingEntity(loanApplicationId, userAccount, fundingEntity)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(
            new CheckAnswersFundingSectionCommand(loanApplicationId, CommonResponse.Yes),
            CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        fundingEntity.Status!.Should().Be(SectionStatus.Completed);
    }
}
