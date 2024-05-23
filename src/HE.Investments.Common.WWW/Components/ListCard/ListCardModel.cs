namespace HE.Investments.Common.WWW.Components.ListCard;

public record ListCardModel(string Header, IList<ListCardItemModel> Items, string? Title = null, string? Description = null, string? ViewAllLabel = null, string? ViewAllUrl = null);
