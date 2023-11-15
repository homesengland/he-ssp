using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class HomeTypeRepository : CrmEntityRepository<invln_HomeType, DataverseContext>, IHomeTypeRepository
    {
        public HomeTypeRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public invln_HomeType GetHomeTypeByIdAndApplicationId(string homeTypeId, string applicationId, string attributes = null)
        {
            var fetchXml = @"<fetch>
                  <entity name=""invln_hometype"">"
                    + attributes +
                    @"<filter>
                      <condition attribute=""invln_hometypeid"" operator=""eq"" value=""" + homeTypeId + @""" />
                      <condition attribute=""invln_application"" operator=""eq"" value=""" + applicationId + @""" />
                    </filter>
                  </entity>
                </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_HomeType>()).AsEnumerable().FirstOrDefault();
        }

        public List<invln_HomeType> GetHomeTypesRelatedToApplication(Guid applicationId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_HomeType>()
                     .Where(x => x.invln_application.Id == applicationId).ToList();
            }
        }
    }
}
