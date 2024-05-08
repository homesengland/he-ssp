namespace HE.Investment.AHP.WWW.Views.ConsortiumMember.Const;

public static class ConsortiumMemberPageTitles
{
    public const string SearchOrganisation = "Search for the name of the organisation you want to add to the consortium";

    public const string SearchResult = "Select an organisation to add to the consortium";

    public const string SearchNoResults = "The details you entered did not match our records";

    public const string AddOrganisation = "Add organisation to consortium";

    public const string AddMembers = "Providers";

    public static string ConsortiumManagement(string leadPartnerName) => $"{leadPartnerName} consortium management";

    public static string ConsortiumMember(string leadPartnerName) => $"{leadPartnerName} consortium";

    public static string RemoveMember(string memberName) => $"Are you sure you want to remove {memberName} from this consortium?";
}
