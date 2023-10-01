using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
internal sealed class ProjectPagesUrls
{
    public const string StartSuffix = "/project/start";

    public const string NameSuffix = "/name";

    public const string StartDateSuffix = "/start-date";

    public const string ManyHomesSuffix = "/many-homes";

    public const string PlanningReferenceNumberExistsSuffix = "/planning-ref-number-exists";

    public const string PlanningReferenceNumberSuffix = "/planning-ref-number";

    public const string PlanningPermissionStatusSuffix = "/planning-permission-status";

    public const string LocationSuffix = "/location";

    public const string CheckAnswersSuffix = "/check-answers";

    public static string Start(string applicationId)
    {
        return $"application/{applicationId}{StartSuffix}";
    }

    public static string Name(string applicationId, string projectId)
    {
        return $"application/{applicationId}/project/{projectId}{NameSuffix}";
    }

    public static string StartDate(string applicationId, string projectId)
    {
        return $"application/{applicationId}/project/{projectId}{StartDateSuffix}";
    }

    public static string PlanningReferenceNumberExists(string applicationId, string projectId)
    {
        return $"application/{applicationId}/project/{projectId}{PlanningReferenceNumberExistsSuffix}";
    }

    public static string PlanningReferenceNumber(string applicationId, string projectId)
    {
        return $"application/{applicationId}/project/{projectId}{PlanningReferenceNumberSuffix}";
    }

    public static string CheckAnswers(string applicationId, string projectId)
    {
        return $"application/{applicationId}/project/{projectId}{CheckAnswersSuffix}";
    }
}
