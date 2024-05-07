using HE.Investments.Account.Contract.Organisation.Events;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Common.Domain;

namespace HE.Investments.Account.Domain.Organisation.Entities;

public class OrganisationEntity : DomainEntity
{
    public OrganisationEntity(OrganisationName name, OrganisationAddress address)
    {
        Name = name;
        Address = address;
        Publish(new OrganisationHasBeenCreatedEvent(name.Name));
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
