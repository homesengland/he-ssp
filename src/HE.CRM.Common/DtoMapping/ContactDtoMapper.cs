using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace HE.CRM.Common.DtoMapping
{
    public class ContactDtoMapper
    {
        public static ContactDto MapRegularEntityToContactDto(Contact contact)
        {
            var contactDto = new ContactDto()
            {
                firstName = contact.FirstName,
                lastName = contact.LastName,
                email = contact.EMailAddress1,
                phoneNumber = contact.Address1_Telephone1,
                secondaryPhoneNumber = contact.Address1_Telephone2,
                jobTitle = contact.JobTitle,
                city = contact.Address1_City,
                county = contact.Address1_County,
                postcode = contact.Address1_PostalCode,
                country = contact.Address1_Country,
                isTermsAndConditionsAccepted = contact.invln_termsandconditionsaccepted,
            };
            if(contact.Id != null)
            {
                contactDto.contactId = contact.Id.ToString();
            }

            return contactDto;
        }

        public static Contact MapContactDtoToRegularEntitry(ContactDto contactDto)
        {
            var contact = new Contact()
            {
                FirstName = contactDto.firstName,
                LastName = contactDto.lastName,
                EMailAddress1 = contactDto.email,
                Address1_Telephone1 = contactDto.phoneNumber,
                Address1_Telephone2 = contactDto.secondaryPhoneNumber,
                JobTitle = contactDto.jobTitle,
                Address1_City = contactDto.city,
                Address1_County = contactDto.county,
                Address1_PostalCode = contactDto.postcode,
                Address1_Country = contactDto.country,
                invln_termsandconditionsaccepted = contactDto.isTermsAndConditionsAccepted,
            };
            if(Guid.TryParse(contactDto.contactId, out Guid recordId))
            {
                contact.Id = recordId;
            }
            return contact;
        }
    }
}
