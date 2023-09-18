using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
internal class ProjectPagesUrls
{
    public const string StartSuffix = "/project/start";

    public const string NameSuffix = "/name";

    public const string StartDateSuffix = "/start-date";

    public static string Name(string applicationId, string projectId)
    {
        return $"application/{applicationId}/project/{projectId}";
    }
}
