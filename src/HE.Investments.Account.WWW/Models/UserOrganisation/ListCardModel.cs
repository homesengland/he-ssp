namespace HE.Investments.Account.WWW.Models.UserOrganisation;

public record ListCardModel(string Header, IList<ListCardItemModel> Items, string? Title = null, string? ViewAllUrl = null);
