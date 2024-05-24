using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Contract.UserOrganisation;

public class UserAppliance
{
    public UserAppliance(HeApplianceId id, string applicationName, ApplicationStatus? status = null)
    {
        Id = id;
        Name = applicationName;
        Status = status;
    }

    public HeApplianceId Id { get; }

    public string Name { get; }

    public ApplicationStatus? Status { get; }
}
