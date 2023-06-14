using System;
using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IContactRepository : ICrmEntityRepository<Contact, DataverseContext>
    {
        Contact GetFirstContactWithGivenEmail(string email);
        List<Contact> GetAllContactsWithGivenEmail(string email);
    }
}
