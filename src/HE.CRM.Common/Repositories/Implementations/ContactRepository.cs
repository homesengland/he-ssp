using System;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using System.Linq;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

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
                        invln_externalid = contactExternalId
                    };
                    service.Create(contactToCreate);
                    return contactToCreate;
                }
            }
        }

        public Contact GetContactViaExternalId(string contactExternalId, string[] columnSet = null)
        {
            var keys = new KeyAttributeCollection
                {
                    { "invln_externalid", contactExternalId }
                };

            var request = new RetrieveRequest
            {
                ColumnSet = new ColumnSet(columnSet),
                Target = new EntityReference(Contact.EntityLogicalName, keys)
            };

            try
            {
                var response = (RetrieveResponse)service.Execute(request);
                if (response != null)
                {
                    return response.Entity.ToEntity<Contact>();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("Contact with invln_externalid: " + contactExternalId + " does not extst in CRM");
            }
        }

        public AssociateResponse ExecuteAssociateRequest(AssociateRequest request)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return (AssociateResponse)ctx.Execute(request);
            }
        }

        public List<Contact> GetContactsForOrganisation(Guid organisationId)
        {
            using (DataverseContext ctx = new DataverseContext(service))
            {
                return (from cnt in ctx.ContactSet
                        join cwr in ctx.invln_contactwebroleSet on cnt.ContactId equals cwr.invln_Contactid.Id
                        join acc in ctx.AccountSet on cwr.invln_Accountid.Id equals acc.Id
                        where acc.AccountId == organisationId
                        select cnt).ToList();

            }
        }

        public List<Contact> GetOrganisationAdministrators(Guid organisationId)
        {
            using (DataverseContext ctx = new DataverseContext(service))
            {
                return (from cnt in ctx.ContactSet
                        join cwr in ctx.invln_contactwebroleSet on cnt.ContactId equals cwr.invln_Contactid.Id
                        join acc in ctx.AccountSet on cwr.invln_Accountid.Id equals acc.Id
                        join wr in ctx.invln_WebroleSet on cwr.invln_Webroleid.Id equals wr.Id
                        join ppl in ctx.invln_portalpermissionlevelSet on wr.invln_Portalpermissionlevelid.Id equals ppl.Id
                        where acc.AccountId == organisationId && ppl.invln_Permission.Value == (int)invln_Permission.Admin
                        select cnt).ToList();

            }
        }
    }
}
