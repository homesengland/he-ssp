using HE.Investments.Common.Extensions;

namespace HE.Investments.Account.Domain.UserOrganisation.Storage.Api;

internal static class ProjectApiUrls
{
    public static string Projects(string organisationId, string? userId = null)
    {
        var url = $"{organisationId.ToGuidAsString()}/projects";
        return string.IsNullOrEmpty(userId) ? url : $"{url}?userId={userId}";
    }
}
