using HE.Investments.Common.Gds;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Views.Shared.Components.LoansFormButton;

public class LoansFormButton : ViewComponent
{
    public IViewComponentResult Invoke(
        string id = "continue-button",
        string text = "Continue",
        string value = "Continue",
        string name = "action",
        ButtonType buttonType = ButtonType.Standard,
        bool isDisabled = false)
    {
        var buttonClass = $"govuk-button {ConstructButtonClass(buttonType)}";
        return View("LoansFormButton", (id, text, value, name, buttonClass, isDisabled));
    }

    private static string ConstructButtonClass(ButtonType buttonType) => buttonType switch
    {
        ButtonType.Secondary => "govuk-button--secondary",
        ButtonType.Warning => "govuk-button--warning",
        ButtonType.Start => "govuk-button--start",
        _ => string.Empty,
    };
}
