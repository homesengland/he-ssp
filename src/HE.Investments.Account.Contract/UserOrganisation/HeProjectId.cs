using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Contract.UserOrganisation;

public record HeProjectId : StringIdValueObject
{
    private HeProjectId(string value)
        : base(value)
    {
    }

    public static HeProjectId From(string value) => new(value);

    public override string ToString()
    {
        return Value;
    }
}
