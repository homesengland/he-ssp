using System;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using System.Linq;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Messages;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ContactRepository : CrmEntityRepository<Contact, DataverseContext>, IContactRepository
    {
        public ContactRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public Contact GetFirstContactWithGivenEmail(string email)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<Contact>()
                    .Where(x => x.EMailAddress1 == email).AsEnumerable().FirstOrDefault();
            }
        }

        public List<Contact> GetAllContactsWithGivenEmail(string email)
        {
            logger.Trace("GetAllContactsWithGivenEmail");
            List<Contact> contacts = new List<Contact>();
            if (email != null)
            {
                logger.Trace("Email not null. Email: " + email);
                QueryExpression qe = new QueryExpression(Contact.EntityLogicalName);
                qe.ColumnSet = new ColumnSet(nameof(Contact.EMailAddress1).ToLower());
                qe.Criteria.AddCondition(nameof(Contact.EMailAddress1).ToLower(), ConditionOperator.Equal, email);
                var result = service.RetrieveMultiple(qe);
                if (result != null && result.Entities.Count > 0)
                {
                    logger.Trace("Contacts: " + result.Entities.Count);
                    contacts.AddRange(result.Entities.Select(x => x.ToEntity<Contact>()));
                }
            }

            return contacts;
        }

        public Contact GetContactWithGivenEmailAndExternalId(string contactEmail, string contactExternalId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                var contact = ctx.CreateQuery<Contact>()
                    //.Where(x => x.EMailAddress1 == contactEmail && x.invln_externalid == contactExternalId).AsEnumerable().FirstOrDefault();
                    .Where(x => x.invln_externalid == contactExternalId).AsEnumerable().FirstOrDefault();

                if (contact != null)
                {
                    logger.Trace("Contact exists");
                    return contact;
                }
                else
                {
                    logger.Trace("Create contact");
                    var contactToCreate = new Contact()
                    {
                        Id = Guid.NewGuid(),
                        EMailAddress1 = contactEmail,
                        invln_externalid = contactExternalId,
                        Telephone1 = "123123123"
                    };
                    service.Create(contactToCreate);
                    return contactToCreate;
                }
            }
        }

        public Contact GetContactViaExternalId(string contactExternalId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                var contact = ctx.CreateQuery<Contact>()
                    .Where(x => x.invln_externalid == contactExternalId).AsEnumerable().FirstOrDefault();

                return contact;
            }
        }

        public AssociateResponse ExecuteAssociateRequest(AssociateRequest request)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return (AssociateResponse)ctx.Execute(request);
            }
        }
    }
}