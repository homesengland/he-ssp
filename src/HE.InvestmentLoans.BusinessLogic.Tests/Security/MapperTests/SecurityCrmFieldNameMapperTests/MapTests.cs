using HE.InvestmentLoans.BusinessLogic.Security.Mappers;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.CRM.Model;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Security.MapperTests.SecurityCrmFieldNameMapperTests;

public class MapTests
{
    [Fact]
    public void ShouldReturnExternalStatusAndSecurityDetailsCompletionStatusWhenSecurityFieldsSetIsGetEmpty()
    {
        // given
        var getEmpty = SecurityFieldsSet.GetEmpty;

        // when
        var result = SecurityCrmFieldNameMapper.Map(getEmpty);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_ExternalStatus).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_securitydetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnDebentureHolderWithOutstandingLegalChargesOrDebtAndSecurityDetailsCompletionStatusWhenSecurityFieldsSetIsChargesDebtCompany()
    {
        // given
        var chargesDebtCompany = SecurityFieldsSet.ChargesDebtCompany;

        // when
        var result = SecurityCrmFieldNameMapper.Map(chargesDebtCompany);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_DebentureHolder).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Outstandinglegalchargesordebt).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_securitydetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnDirectorLoansAndSecurityDetailsCompletionStatusWhenSecurityFieldsSetIsDirLoans()
    {
        // given
        var dirLoans = SecurityFieldsSet.DirLoans;

        // when
        var result = SecurityCrmFieldNameMapper.Map(dirLoans);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_Directorloans).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_securitydetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void
        ShouldReturnConfirmationDirectorLoansCanBeSubordinatedWithReasonForDirectorLoanNotSubordinatedAndSecurityDetailsCompletionStatusWhenSecurityFieldsSetIsDirLoansSub()
    {
        // given
        var dirLoansSub = SecurityFieldsSet.DirLoansSub;

        // when
        var result = SecurityCrmFieldNameMapper.Map(dirLoansSub);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_Confirmationdirectorloanscanbesubordinated).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Reasonfordirectorloannotsubordinated).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_securitydetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnAllFieldsWhenSecurityFieldsSetIsGetAllFields()
    {
        // given
        var getAllFields = SecurityFieldsSet.GetAllFields;

        // when
        var result = SecurityCrmFieldNameMapper.Map(getAllFields);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_ExternalStatus).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_DebentureHolder).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Outstandinglegalchargesordebt).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Directorloans).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Confirmationdirectorloanscanbesubordinated).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Reasonfordirectorloannotsubordinated).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_securitydetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnAllFieldsExceptExternalStatusWhenSecurityFieldsSetIsSaveAllFields()
    {
        // given
        var saveAllFields = SecurityFieldsSet.SaveAllFields;

        // when
        var result = SecurityCrmFieldNameMapper.Map(saveAllFields);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_DebentureHolder).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Outstandinglegalchargesordebt).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Directorloans).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Confirmationdirectorloanscanbesubordinated).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Reasonfordirectorloannotsubordinated).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_securitydetailscompletionstatus).ToLowerInvariant()}");
    }
}
