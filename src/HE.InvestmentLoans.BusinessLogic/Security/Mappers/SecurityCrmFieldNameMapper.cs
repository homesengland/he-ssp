using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.CRM.Model;

namespace HE.InvestmentLoans.BusinessLogic.Security.Mappers;

public static class SecurityCrmFieldNameMapper
{
    private static readonly string DebentureHolder = $"{nameof(invln_Loanapplication.invln_DebentureHolder).ToLowerInvariant()},";
    private static readonly string OutstandingLegalChargesOrDebt = $"{nameof(invln_Loanapplication.invln_Outstandinglegalchargesordebt).ToLowerInvariant()},";
    private static readonly string DirectorLoans = $"{nameof(invln_Loanapplication.invln_Directorloans).ToLowerInvariant()},";
    private static readonly string ConfirmationDirectorLoansCanBeSubordinated = $"{nameof(invln_Loanapplication.invln_Confirmationdirectorloanscanbesubordinated).ToLowerInvariant()},";
    private static readonly string ReasonForDirectorLoanNotSubordinated = $"{nameof(invln_Loanapplication.invln_Reasonfordirectorloannotsubordinated).ToLowerInvariant()},";
    private static readonly string SecurityDetailsCompletionStatus = $"{nameof(invln_Loanapplication.invln_securitydetailscompletionstatus).ToLowerInvariant()}";

    public static string Map(SecurityViewOption securityViewOption)
    {
        var result = securityViewOption switch
        {
            SecurityViewOption.ChargesDebtCompany => DebentureHolder + OutstandingLegalChargesOrDebt,
            SecurityViewOption.DirLoans => DirectorLoans,
            SecurityViewOption.DirLoansSub => ConfirmationDirectorLoansCanBeSubordinated + ReasonForDirectorLoanNotSubordinated,
            SecurityViewOption.GetAllFields => DebentureHolder +
                                               OutstandingLegalChargesOrDebt +
                                               DirectorLoans +
                                               ConfirmationDirectorLoansCanBeSubordinated +
                                               ReasonForDirectorLoanNotSubordinated,
            _ => string.Empty,
        };

        return result + SecurityDetailsCompletionStatus;
    }
}
