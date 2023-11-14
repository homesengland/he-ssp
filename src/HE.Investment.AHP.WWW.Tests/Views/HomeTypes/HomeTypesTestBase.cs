using AngleSharp.Html.Dom;
using HE.Investments.Common.Tests.WWW;
using HE.Investments.Common.Tests.WWW.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public abstract class HomeTypesTestBase : ViewTestBase
{
    protected const string ErrorMessage = "Some error message";

    protected void AssertErrors(IHtmlDocument document, string fieldName, bool hasError)
    {
        document.HasSummaryErrorMessage(fieldName, ErrorMessage, hasError)
            .HasErrorMessage(fieldName, ErrorMessage, hasError);
    }
}
