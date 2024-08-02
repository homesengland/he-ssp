namespace HE.Investments.TestsUtils.Extensions.Models;

public record ListCardModel(string Title, ListCardHeaderAction? Action, IList<ListCardContent> ContentList);

public record ListCardHeaderAction(string Title, string Url);

public record ListCardContent(string Title, string? Description, IList<ListCardContentItem> Items);

public record ListCardContentItem(string Name, string ActionUrl);
