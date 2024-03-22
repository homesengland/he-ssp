using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;


namespace HE.CRM.Common.Repositories.Implementations
{
    public class HeProjectRepository : CrmEntityRepository<he_Pipeline, DataverseContext>, IHeProjectRepository
    {
        public HeProjectRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<he_Pipeline> GetHeProject(string organisationCondition, string contactExternalIdFilter, string frontDoorProjectFilters, string statecodeCondition)
        {
            logger.Trace("FrontDoorProjectRepository GetFrontDoorProjectForOrganisationAndContact");
            var fetchXML = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
	                                <entity name='he_pipeline'>
		                                    <attribute name=""he_pipelineid"" />
		                                    <attribute name=""he_amountofaffordablehomes"" />
		                                    <attribute name=""he_amountofaffordablehomesname"" />
		                                    <attribute name=""he_doyouhaveanidentifiedsite"" />
		                                    <attribute name=""he_doyouhaveanidentifiedsitename"" />
		                                    <attribute name=""he_doyourequirefundingforyourproject"" />
		                                    <attribute name=""he_doyourequirefundingforyourprojectname"" />
		                                    <attribute name=""he_howmanyhomeswillyourprojectenable"" />
		                                    <attribute name=""he_howmuchfundingdoyourequired"" />
		                                    <attribute name=""he_howmuchfundingdoyourequiredname"" />
		                                    <attribute name=""he_intentiontomakeaprofit"" />
		                                    <attribute name=""he_intentiontomakeaprofitname"" />
		                                    <attribute name=""he_account"" />
		                                    <attribute name=""he_accountname"" />
		                                    <attribute name=""he_name"" />
		                                    <attribute name=""he_portalowner"" />
		                                    <attribute name=""he_portalownername"" />
		                                    <attribute name=""he_previousresidentialbuildingexperience"" />
		                                    <attribute name=""he_projectprogressmoreslowly"" />
		                                    <attribute name=""he_projectprogressmoreslowlyname"" />
		                                    <attribute name=""he_projectname"" />
		                                    <attribute name=""he_startofprojectmonth"" />
		                                    <attribute name=""he_startofprojectyear"" />
		                                    <attribute name=""statecode"" />
		                                    <attribute name=""statecodename"" />
		                                    <attribute name=""he_he_infrastructuredoesyourprojectdeliver"" />
		                                    <attribute name=""he_he_infrastructuredoesyourprojectdelivername"" />
		                                    <attribute name=""he_whatisthegeographicfocusoftheproject"" />
		                                    <attribute name=""he_whatisthegeographicfocusoftheprojectname"" />
		                                    <attribute name=""he_projectbelocated"" />
		                                    <attribute name=""he_projectbelocatedname"" />
		                                    <attribute name=""he_regionlocation"" />
		                                    <attribute name=""he_regionlocationname"" />
                                            <attribute name=""he_activitiesinthisproject"" />
                                            <attribute name=""he_housingdeliveryinengland"" />
                                             <filter>
                                              {statecodeCondition}
                                              {organisationCondition}
                                              {frontDoorProjectFilters}
                                             </filter>
                                             <link-entity name=""contact"" from=""contactid"" to=""he_portalowner"">
                                              {contactExternalIdFilter}
                                            </link-entity>
                                          </entity>
                                        </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXML));
            return result.Entities.Select(x => x.ToEntity<he_Pipeline>()).ToList();
        }

        public bool CheckIfHeProjectWithGivenNameExists(string frontDoorProjectName, Guid organisationId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                if (organisationId != Guid.Empty)
                {
                    return ctx.CreateQuery<he_Pipeline>().Where(x => x.he_ProjectName  == frontDoorProjectName && x.he_Account.Id == organisationId && x.StateCode.Value == (int)he_PipelineState.Active).AsEnumerable().Any();
                }
                else
                {
                    return ctx.CreateQuery<he_Pipeline>().Where(x => x.he_ProjectName == frontDoorProjectName && x.StateCode.Value == (int)he_PipelineState.Active).AsEnumerable().Any();
                }
            }
        }
    }
}
