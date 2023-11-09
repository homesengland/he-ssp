using HE.Investments.Account.Domain.Organisation.ValueObjects;

namespace HE.Investments.Account.Domain.Organisation.Entities;

public class OrganisationEntity
{
    public OrganisationEntity(OrganisationName name, OrganisationAddress address)
    {
        Name = name;
        Address = address;
    }

    public OrganisationEntity(OrganisationName name, OrganisationAddress address, OrganisationPhoneNumber phoneNumber)
    {
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
    }

    public OrganisationName Name { get; private set; }

    public OrganisationAddress Address { get; private set; }

    public OrganisationPhoneNumber PhoneNumber { get; private set; }
}
