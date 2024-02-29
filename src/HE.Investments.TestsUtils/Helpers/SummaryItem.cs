using AngleSharp.Html.Dom;

namespace HE.Investments.TestsUtils.Helpers;

public record SummaryItem(string Header, string Title, string Value, IHtmlAnchorElement? ChangeAnswerLink);
