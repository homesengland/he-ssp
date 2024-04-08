namespace HE.Investments.Common.WWW.Config;

public class MvcAppConfig : IMvcAppConfig
{
    public string Environment { get; set; }

    public bool UseEnvironmentSuffixInAppTitle { get; set; }

    public string BuildAppTitle(string title)
    {
        if (!UseEnvironmentSuffixInAppTitle)
        {
            return title;
        }

        return $"{title} ({Environment})";
    }
}
