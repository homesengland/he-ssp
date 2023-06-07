using System;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using System.Linq;
using Microsoft.Xrm.Sdk.Client;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ContactRepository : CrmEntityRepository<Contact, DataverseContext>, IContactRepository
    {
        public ContactRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public Contact GetContactWithGivenEmail(string email)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<Contact>()
                    .Where(x => x.EMailAddress1 == email).AsEnumerable().FirstOrDefault();
            }
        }
    }
}