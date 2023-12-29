using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Delivery;
public class Delivery
{
    public Guid ApplicationId { get; set; }

    public string ApplicationName { get; set; }

    public SectionStatus SectionStatus { get; set; }
}
