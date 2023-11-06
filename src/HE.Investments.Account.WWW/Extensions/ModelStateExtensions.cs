using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investments.Account.WWW.Extensions;

public static class ModelStateExtensions
{
    public static string GetErrorMessage(this ModelStateEntry modelStateEntry)
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
