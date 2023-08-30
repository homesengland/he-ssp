using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.InvestmentLoans.Common.Validation;

public static class ModelStateDictionaryExtensions
{
    public static void AddValidationErrors(this ModelStateDictionary modelState, OperationResult result)
    {
        if (result.IsValid)
        {
            return;
        }

        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.AffectedField, error.ErrorMessage);
        }
    }
}
