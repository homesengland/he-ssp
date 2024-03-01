using System.Linq;

namespace HE.CRM.Common.Helpers
{
    public class FetchXmlHelper
    {
        public static string GenerateAttributes(string fieldsToRetrieve)
        {
            var items = fieldsToRetrieve.Split(',');
            var fields = string.Join(string.Empty, items.Select(f => $"<attribute name=\"{f}\" />"));
            return fields;
        }
    }
}
