using HE.Base.Repositories;
using DataverseModel;
using Microsoft.Xrm.Sdk;
using System;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IGlobalRepository : ICrmEntityRepository<Entity, DataverseContext>
    {
        Entity RetrieveEntityOfGivenTypeWithGivenFields(string entityName, Guid entityId, string[] fields);
    }
}