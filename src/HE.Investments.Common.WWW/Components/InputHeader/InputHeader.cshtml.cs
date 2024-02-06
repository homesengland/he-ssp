using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.InputHeader;

public class InputHeader : ViewComponent
{
    /// <summary>
    /// Generates view component for single input header.
    /// </summary>
    /// <param name="fieldName">Filed name used to assign for attribute of label html element.</param>
    /// <param name="header">Value should be provided when we want to display input title label as page header.</param>
    /// <param name="title">Value should be provided when we want to display input title label in standard way.</param>
    /// <param name="description">Value should be provided when we want to display input title label not as a header.</param>
    /// <param name="hint">Hint text for input.</param>
    /// <returns>Input header view component.</returns>
    public IViewComponentResult Invoke(
        string fieldName,
        string? header = null,
        string? title = null,
        string? description = null,
        string? hint = null)
    {
        // We may need to display field title as a page heading when there is only one input on the page.
        // Is important to display vertical red line next to header in case of error.
        // Please use header param when you need to display field title as a page header otherwise use title param.
        return View("InputHeader", (fieldName, header, title, description, hint));
    }
}
