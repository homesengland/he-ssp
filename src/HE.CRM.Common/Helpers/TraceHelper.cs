namespace HE.CRM.Common.Helpers
{
    public static class TraceHelper
    {
        public static string AddHeader(string message)
        {
            return $"{message} --------------------------------------------------------------";
        }
    }
}
