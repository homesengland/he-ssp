using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.InvestmentLoans.Contract.Application.Enums;
using Microsoft.Xrm.Sdk.Metadata;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.LoanApplicationEntityTests;
public class CanBeSubmittedTests
{
    [Fact]
    public void CannotBeSubmitted_WhenCompanyStructureSectionIsNotCompleted()
    {
        var entity = LoanApplicationTestBuilder
            .NewDraft()
            .WithCompanyStructureSection(LoanApplicationSectionTestData.InCompletedSection)
            .Build();

        entity.CanBeSubmitted().Should().BeFalse();
    }

    [Fact]
    public void CannotBeSubmitted_WhenSecuritySectionIsNotCompleted()
    {
        var entity = LoanApplicationTestBuilder
            .NewDraft()
            .WithCompanyStructureSection(LoanApplicationSectionTestData.CompletedSection)
            .WithSecuritySection(LoanApplicationSectionTestData.InCompletedSection)
            .Build();

        entity.CanBeSubmitted().Should().BeFalse();
    }
}
