using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Extensions;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Tags;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.Components;

public class SectionStatusTagHelper : TextWithIdTagHelper
{
    public SectionStatusTagHelper()
        : base(HtmlConstants.Strong, string.Empty)
    {
    }

    public SectionStatus Status { get; set; }

    public string AdditionalClasses { get; set; }

    protected override void ConstructClass(TagHelperOutput output)
    {
        Text = Status.GetDescription();
        TagConstruct.ConstructClass(output, $"{CssConstants.GovUkTag} {TagColourClass(Status)} {AdditionalClasses}");
    }

    private string TagColourClass(SectionStatus status)
    {
        var tagColor = GetColorBaseOnStatus(status)?.ToString().ToLowerInvariant();
        if (tagColor is null)
        {
            return "govuk-tag";
        }

        return $"govuk-tag--{tagColor}";
    }

    private TagColour? GetColorBaseOnStatus(SectionStatus status)
    {
        return status switch
        {
            SectionStatus.NotStarted => TagColour.Grey,
            SectionStatus.InProgress => TagColour.Blue,
            SectionStatus.Submitted => TagColour.Green,
            SectionStatus.NotSubmitted => TagColour.Red,
            SectionStatus.Withdrawn => TagColour.Red,
            SectionStatus.Completed => null,
            _ => TagColour.Grey,
        };
    }
}
