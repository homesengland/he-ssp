using System;
using HE.Base.Repositories;
using DataverseModel;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IContactRepository : ICrmEntityRepository<Contact, DataverseContext>
    {
        Contact GetContactWithGivenEmail(string email);
    }
}
