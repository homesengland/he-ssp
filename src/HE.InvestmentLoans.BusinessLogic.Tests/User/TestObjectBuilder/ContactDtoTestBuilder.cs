extern alias Org;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
public class ContactDtoTestBuilder
{
    private readonly ContactDto _item;

    private ContactDtoTestBuilder(ContactDto contactDto)
    {
        _item = contactDto;
    }

    public static ContactDtoTestBuilder New()
    {
        return new(new ContactDto()
        {
            firstName = ContactDtoTestData.ContactDto.firstName,
            lastName = ContactDtoTestData.ContactDto.lastName,
            jobTitle = ContactDtoTestData.ContactDto.jobTitle,
            email = ContactDtoTestData.ContactDto.email,
            phoneNumber = ContactDtoTestData.ContactDto.phoneNumber,
            secondaryPhoneNumber = ContactDtoTestData.ContactDto.secondaryPhoneNumber,
            isTermsAndConditionsAccepted = ContactDtoTestData.ContactDto.isTermsAndConditionsAccepted,
        });
    }

    public ContactDto Build()
    {
        return _item;
    }
}
