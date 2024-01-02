namespace HE.Investment.AHP.WWW.Models.Delivery;

public class DeliveryModelBase
{
    public DeliveryModelBase(string applicationName)
    {
        ApplicationName = applicationName;
    }

    public string ApplicationName { get; set; }
}
