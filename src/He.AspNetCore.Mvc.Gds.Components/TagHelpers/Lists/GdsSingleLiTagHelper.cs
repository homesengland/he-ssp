using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Lists
{
    public class GdsSingleLiTagHelper : TextTagHelper
    {
        public GdsSingleLiTagHelper() : base(HtmlConstants.Li, $"{CssConstants.GovUkList} {CssConstants.GovUkListBullet}")
        {
        }
    }
}
