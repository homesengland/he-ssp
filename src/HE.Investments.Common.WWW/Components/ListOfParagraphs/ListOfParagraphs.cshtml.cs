using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.ListOfParagraphs;

public class ListOfParagraphs : ViewComponent
{
    public IViewComponentResult Invoke(IList<string> paragraphs)
    {
        return View("ListOfParagraphs", paragraphs);
    }
}
