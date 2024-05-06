using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investments.Common.WWW.Extensions;

public static class ModelStateExtensions
{
    public static (bool HasAnyError, string Messge) GetErrors(this ModelStateDictionary? modelState, string key)
    {
        if (modelState is null)
        {
            return (false, string.Empty);
        }

        var hasError = modelState.GetFieldValidationState(key) == ModelValidationState.Invalid;
        if (!hasError)
        {
            return (false, string.Empty);
        }

        return (true, modelState[key]!.GetErrorMessage());
    }

    public static Dictionary<string, string> GetOrderedErrors(this ModelStateDictionary modelState, List<string> orderedKeys)
    {
        return modelState
            .ToDictionary(i => i.Key, i => GetErrorMessage(i.Value))
            .OrderBy(i =>
            {
                var index = i.Key.IndexOf('[');
                return orderedKeys.IndexOf(i.Key.Remove(index < 0 ? i.Key.Length : index));
            })
            .ToDictionary(x => x.Key, x => x.Value);
    }

    public static string GetErrorMessage(this ModelStateEntry? modelStateEntry)
    {
        if (modelStateEntry is null)
        {
            return string.Empty;
        }

        var hasError = modelStateEntry.ValidationState == ModelValidationState.Invalid;

        if (!hasError)
        {
            return string.Empty;
        }

        return modelStateEntry.Errors.Aggregate(new StringBuilder(), (sb, next) => sb.AppendLine(next.ErrorMessage)).ToString();
    }

    public static bool HasError(this ModelStateDictionary? modelState, string key)
    {
        if (modelState is null)
        {
            return false;
        }

        return modelState.GetFieldValidationState(key) == ModelValidationState.Invalid;
    }
}
