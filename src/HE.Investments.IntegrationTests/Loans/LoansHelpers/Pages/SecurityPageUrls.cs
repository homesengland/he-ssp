using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
internal sealed class SecurityPageUrls
{
    public const string StartSuffix = "/security";

    public const string ChargesDebtSuffix = "/security/charges-debt";

    public const string DirectorLoansSuffix = "/security/dir-loans";

    public const string DirLoansSubSuffix = "/security/dir-loans-sub";

    public const string CheckYourAnswersSuffix = "/security/check-answers";

    public static string ChargesDebt(string applicationId) => $"/application/{applicationId}{ChargesDebtSuffix}";

    public static string DirectorLoans(string applicationId) => $"/application/{applicationId}{DirectorLoansSuffix}";

    public static string DirLoansSub(string applicationId) => $"/application/{applicationId}{DirLoansSubSuffix}";

    public static string CheckYourAnswers(string applicationId) => $"/application/{applicationId}{CheckYourAnswersSuffix}";
}
