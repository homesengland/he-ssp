using HE.Investments.Account.Shared.Authorization.Attributes;

namespace HE.Investments.Consortium.Shared.Authorization;

[AttributeUsage(AttributeTargets.All)]
public class ConsortiumAuthorizeAttribute(string? allowedFor = null)
    : AuthorizeWithCompletedProfileAttribute(allowedFor ?? string.Empty, typeof(ConsortiumAccessPolicy));
