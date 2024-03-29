using System.Collections.Generic;

namespace He.AspNetCore.Mvc.Gds.Components.Constants
{
    public static class CommonResponse
    {
        public const string Yes = "Yes";
        public const string No = "No";
        public const string DoNotKnow = "DoNotKnow";

        public static IEnumerable<string> YesNoAnswers()
        {
            yield return Yes;
            yield return No;
        }
    }
}

