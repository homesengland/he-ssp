using System.Text;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Pagination
{
    public class GdsPaginationTagHelper : TagHelper
    {
        public GdsPaginationTagHelper()
        {

        }

        public int TotalItems { get; set; }

        public int Page { get; set; }

        public int ItemsPerPage { get; set; }

        public string BaseUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (output == null)
            {
                return;
            }

            output.TagName = HtmlConstants.Nav;

            TagConstruct.ConstructClass(output, CssConstants.GovUkPagination);
            TagConstruct.ConstructGenericAttribute(output, HtmlAttributes.AriaAttributes.Label, "results");

            if (TotalItems <= 0)
            {
                return;
            }

            var contentBuilder = new StringBuilder();

            var totalPages = (TotalItems / ItemsPerPage) + ((TotalItems % ItemsPerPage) > 0 ? 1 : 0);

            AdjustPageToCorrectRange(totalPages);

            if (Page > 1 && totalPages > 1)
            {
                AddPreviousButton(contentBuilder);
            }

            AddPageButtons(contentBuilder, totalPages);

            if (Page < totalPages)
            {
                AddNextButton(contentBuilder);
            }

            output.Content.SetHtmlContent(contentBuilder.ToString());
        }

        private void AddPageButtons(StringBuilder contentBuilder, int totalPages)
        {
            contentBuilder.Append("<ul class=\"govuk-pagination__list\">");

            var selectedPageDistanceFromFirstPage = Page - 1;
            var selectedPageDistanceFromLastPage = totalPages - Page;

            if (selectedPageDistanceFromFirstPage != 0)
            {
                AddPageItem(contentBuilder, 1);
            }

            if (selectedPageDistanceFromFirstPage > 2)
            {
                AddElipse(contentBuilder);
            }

            if (selectedPageDistanceFromFirstPage > 1)
            {
                AddPageItem(contentBuilder, Page - 1);
            }

            AddSelectedPageItem(contentBuilder);

            if (selectedPageDistanceFromLastPage > 1)
            {
                AddPageItem(contentBuilder, Page + 1);
            }

            if (selectedPageDistanceFromLastPage > 2)
            {
                AddElipse(contentBuilder);
            }

            if (selectedPageDistanceFromLastPage != 0)
            {
                AddPageItem(contentBuilder, totalPages);
            }

            contentBuilder.Append("</ul>");
        }

        private void AdjustPageToCorrectRange(int totalPages)
        {
            if (Page < 1)
            {
                Page = 1;
            }

            if (Page > totalPages)
            {
                Page = totalPages;
            }
        }

        private void AddNextButton(StringBuilder contentBuilder)
        {
            contentBuilder.Append($@"
                    <div class=""govuk-pagination__next"">
                        <a class=""govuk-link govuk-pagination__link"" href=""{PageRedirect(Page + 1)}"" rel=""next"">
                            <span class=""govuk-pagination__link-title"">Next</span>
                            <svg class=""govuk-pagination__icon govuk-pagination__icon--next"" xmlns=""http://www.w3.org/2000/svg"" height=""13"" width=""15"" aria-hidden=""true"" focusable=""false"" viewBox=""0 0 15 13"">
                                <path d=""m8.107-0.0078125-1.4136 1.414 4.2926 4.293h-12.986v2h12.896l-4.1855 3.9766 1.377 1.4492 6.7441-6.4062-6.7246-6.7266z""></path>
                            </svg>
                        </a>
                    </div>
                ");
        }

        private void AddPreviousButton(StringBuilder contentBuilder)
        {
            contentBuilder.Append($@"
                    <div class=""govuk-pagination__prev"">
                        <a class=""govuk-link govuk-pagination__link"" href=""{PageRedirect(Page - 1)}"" rel=""prev"">
                            <svg class=""govuk-pagination__icon govuk-pagination__icon--prev"" xmlns=""http://www.w3.org/2000/svg"" height=""13"" width=""15"" aria-hidden=""true"" focusable=""false"" viewBox=""0 0 15 13"">
                                <path d=""m6.5938-0.0078125-6.7266 6.7266 6.7441 6.4062 1.377-1.449-4.1856-3.9768h12.896v-2h-12.984l4.2931-4.293-1.414-1.414z""></path>
                            </svg>
                            <span class=""govuk-pagination__link-title"">Previous</span>
                        </a>
                    </div>
                ");
        }

        private void AddSelectedPageItem(StringBuilder contentBuilder)
        {
            contentBuilder.Append($@"
                <li class=""govuk-pagination__item govuk-pagination__item--current"">
                   <a class=""govuk-link govuk-pagination__link"" href=""{PageRedirect(Page)}"" aria-label=""Page {Page}"" aria-current=""page"">
                    {Page}
                   </a>
                </li>
            ");
        }

        private void AddPageItem(StringBuilder contentBuilder, int pageNumber)
        {
            contentBuilder.Append($@"
                <li class=""govuk-pagination__item"">
                   <a class=""govuk-link govuk-pagination__link"" href=""{PageRedirect(pageNumber)}"" aria-label=""Page {pageNumber}"">
                    {pageNumber}
                   </a>
                </li>
            ");
        }

        private void AddElipse(StringBuilder contentBuilder)
        {
            contentBuilder.Append("<li class=\"govuk-pagination__item govuk-pagination__item--ellipses\">&ctdot;</li>");
        }

        private string PageRedirect(int pageNumber)
        {
            if (BaseUrl.Contains("?"))
            {
                return $"{BaseUrl}&page={pageNumber}";
            }

            return $"{BaseUrl}?page={pageNumber}";
        }
    }
}
