namespace HE.Investments.Loans.Common.Utils.Constants.FormOption;
public static class CommonResponse
{
    public const string Yes = "Yes";
    public const string No = "No";
    public const string DoNotKnow = "DoNotKnow";
    public const string Other = "other";

    public static class Lowercase
    {
#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members
        public const string Yes = "yes";
        public const string No = "no";
        public const string DoNotKnow = "donotknow";
#pragma warning restore S3218 // Inner class members should not shadow outer class "static" or type members
    }
}
