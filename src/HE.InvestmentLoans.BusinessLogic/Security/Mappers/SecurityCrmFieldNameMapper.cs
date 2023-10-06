using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.CRM.Model;

namespace HE.InvestmentLoans.BusinessLogic.Security.Mappers;

public static class SecurityCrmFieldNameMapper
{
    private static readonly string ExternalStatus = $"{nameof(invln_Loanapplication.invln_ExternalStatus).ToLowerInvariant()},";
    private static readonly string DebentureHolder = $"{nameof(invln_Loanapplication.invln_DebentureHolder).ToLowerInvariant()},";
    private static readonly string OutstandingLegalChargesOrDebt = $"{nameof(invln_Loanapplication.invln_Outstandinglegalchargesordebt).ToLowerInvariant()},";
    private static readonly string DirectorLoans = $"{nameof(invln_Loanapplication.invln_Directorloans).ToLowerInvariant()},";

    private static readonly string ConfirmationDirectorLoansCanBeSubordinated =
        $"{nameof(invln_Loanapplication.invln_Confirmationdirectorloanscanbesubordinated).ToLowerInvariant()},";

    private static readonly string ReasonForDirectorLoanNotSubordinated =
        $"{nameof(invln_Loanapplication.invln_Reasonfordirectorloannotsubordinated).ToLowerInvariant()},";

    private static readonly string SecurityDetailsCompletionStatus =
        $"{nameof(invln_Loanapplication.invln_securitydetailscompletionstatus).ToLowerInvariant()}";

    public static string Map(SecurityFieldsSet securityFieldsSet)
    {
        var result = securityFieldsSet switch
        {
            SecurityFieldsSet.GetEmpty => ExternalStatus,
            SecurityFieldsSet.ChargesDebtCompany => DebentureHolder + OutstandingLegalChargesOrDebt,
            SecurityFieldsSet.DirLoans => DirectorLoans,
            SecurityFieldsSet.DirLoansSub => ConfirmationDirectorLoansCanBeSubordinated + ReasonForDirectorLoanNotSubordinated,
            SecurityFieldsSet.GetAllFields => ExternalStatus +
                                              DebentureHolder +
                                              OutstandingLegalChargesOrDebt +
                                              DirectorLoans +
                                              ConfirmationDirectorLoansCanBeSubordinated +
                                              ReasonForDirectorLoanNotSubordinated,
            SecurityFieldsSet.SaveAllFields => DebentureHolder +
                                               OutstandingLegalChargesOrDebt +
                                               DirectorLoans +
                                               ConfirmationDirectorLoansCanBeSubordinated +
                                               ReasonForDirectorLoanNotSubordinated,
            _ => string.Empty,
        };

        return result + SecurityDetailsCompletionStatus;
    }
}
