namespace HE.Investments.TestsUtils.Extensions.Models;

public record ListCardModel(string Title, ListCardHeaderAction? Action);

public record ListCardHeaderAction(string Title, string Url);
