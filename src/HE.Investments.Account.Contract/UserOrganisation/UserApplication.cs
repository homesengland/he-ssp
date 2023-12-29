using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Contract.UserOrganisation;

public class UserApplication
{
    public UserApplication(string id, string applicationName, ApplicationStatus status)
    {
        Id = id;
        ApplicationName = applicationName;
        Status = status;
    }

    public string Id { get; }

    public string ApplicationName { get; }

    public ApplicationStatus Status { get; }
}
