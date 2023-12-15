using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;

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

    public static Dictionary<string, string>? GetOrderedErrors(this ModelStateDictionary? modelState, List<string> orderedKeys)
    {
        var result = new Dictionary<string, string>();

        if (modelState is null)
        {
            return result;
        }

        foreach (var key in orderedKeys)
        {
            var (hasError, errorMsg) = GetErrors(modelState, key);

            if (hasError)
            {
                result.TryAdd(key, errorMsg);
            }
        }

        return result;
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
}
