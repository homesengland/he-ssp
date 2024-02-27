using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class SiteRepository : CrmEntityRepository<invln_Sites, DataverseContext>, ISiteRepository
    {
        public SiteRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public invln_Sites GetById(string id, string fieldsToRetrieve)
        {
            return Get(fieldsToRetrieve, id).FirstOrDefault();
        }

        public List<invln_Sites> GetAll(string fieldsToRetrieve)
        {
            return Get(fieldsToRetrieve);
        }

        private List<invln_Sites> Get(string fieldsToRetrieve, string id = null)
        {
            var fields = GenerateAttributes(fieldsToRetrieve);
            var filter = GenerateIdFilter(id);

            var fetchXml =
                $@"<fetch>
	                <entity name=""invln_sites"">
                        {fields}
                        <order attribute=""createdon"" descending=""true"" />
                        {filter}
                        <link-entity name=""invln_ahglocalauthorities"" to=""invln_localauthority"" from=""invln_ahglocalauthoritiesid""  link-type=""outer"">
                            <attribute name=""invln_gsscode"" />
                        </link-entity>
	                </entity>
                </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_Sites>()).AsEnumerable().ToList();
        }

        private static string GenerateAttributes(string fieldsToRetrieve)
        {
            var items = fieldsToRetrieve.Split(',');
            var fields = string.Join(string.Empty, items.Select(f => $"<attribute name=\"{f}\" />"));
            return fields;
        }

        private static string GenerateIdFilter(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return string.Empty;
            }

            return @"<filter type=""and"">
			                <condition attribute=""invln_sitesid"" operator=""eq"" value=""" + id + @""" />
		                </filter>";
        }
    }
}
