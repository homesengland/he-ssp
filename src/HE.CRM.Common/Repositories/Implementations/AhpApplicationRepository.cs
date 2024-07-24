using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class AhpApplicationRepository : CrmEntityRepository<invln_scheme, DataverseContext>, IAhpApplicationRepository
    {
        public AhpApplicationRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public bool ApplicationWithGivenIdExistsForOrganisation(Guid applicationId, Guid organisationId)
        {
            using (DataverseContext ctx = new DataverseContext(service))
            {
                return (from app in ctx.invln_schemeSet
                        where app.invln_schemeId == applicationId && app.invln_organisationid.Id == organisationId
                        select app).AsEnumerable().Any();

            }
        }

        public bool ApplicationWithGivenNameAndOrganisationExists(string name, Guid organisationId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_scheme>()
                    .Where(x => x.invln_schemename == name && x.invln_organisationid.Id == organisationId).AsEnumerable().Any();
            }
        }

        public bool ApplicationWithGivenNameExists(string name)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_scheme>()
                    .Where(x => x.invln_schemename == name).AsEnumerable().Any();
            }
        }

        public List<invln_scheme> GetApplicationsForOrganisationAndContact(string organisationId, string contactFilter, string attributes, string additionalRecordFilters)
        {
            var fetchXml = $@"<fetch>
                              <entity name=""invln_scheme"">
                                <attribute name=""invln_contactid""/>
                                {attributes}
                                <filter>
                                  <condition attribute= ""invln_organisationid"" operator=""eq"" value = ""{organisationId}"" />
                                  <condition attribute = ""invln_externalstatus"" operator= ""ne"" value = ""{(int)invln_ExternalStatusAHP.Deleted}"" />
                                  {additionalRecordFilters}
                                </filter>
                                    <link-entity name=""contact"" from=""contactid"" to=""invln_contactid"">
                                       {contactFilter}
                                    </link-entity>
                                </entity>
                            </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_scheme>()).ToList();
        }

        public List<invln_scheme> GetRecordsFromAhpApplicationsForAhpProject(Guid ahpProjectGuid, invln_Permission contactWebRole, Contact contact, Guid organisationGuid, bool isLeadPartner, bool IsSitePartner, bool isAllocation, string consortiumId = null)
        {
            var query_invln_sites_invln_ahpprojectid = ahpProjectGuid.ToString();

            var query = new QueryExpression(invln_scheme.EntityLogicalName);
            query.Distinct = true;
            query.AddOrder(invln_scheme.Fields.invln_lastexternalmodificationon, OrderType.Descending);
            query.ColumnSet.AddColumns(
                invln_scheme.Fields.invln_schemeId,
                invln_scheme.Fields.invln_applicationid,
                invln_scheme.Fields.invln_schemename,
                invln_scheme.Fields.invln_ExternalStatus,
                invln_scheme.Fields.invln_Tenure,
                invln_scheme.Fields.invln_lastexternalmodificationon,
                invln_scheme.Fields.invln_fundingrequired,
                invln_scheme.Fields.invln_noofhomes,
                invln_scheme.Fields.invln_DevelopingPartner,
                invln_scheme.Fields.invln_OwneroftheLand,
                invln_scheme.Fields.invln_OwneroftheHomes,
                invln_scheme.Fields.invln_Site,
                invln_scheme.Fields.invln_programmelookup,
                invln_scheme.Fields.invln_HELocalAuthorityID
                );

            var query_invln_sites = query.AddLink(
                invln_Sites.EntityLogicalName,
                invln_scheme.Fields.invln_Site,
                invln_Sites.Fields.invln_SitesId);

            query_invln_sites.LinkCriteria.AddCondition(invln_Sites.Fields.invln_AHPProjectId, ConditionOperator.Equal, query_invln_sites_invln_ahpprojectid);

            if (isAllocation == true)
            {
                query.Criteria.AddCondition(invln_scheme.Fields.invln_isallocation, ConditionOperator.Equal, true);
            }
            else
            {
                query.Criteria.AddCondition(invln_scheme.Fields.invln_isallocation, ConditionOperator.NotEqual, true);
            }

            if (consortiumId == null)
            {
                if (contactWebRole == invln_Permission.Limiteduser)
                {
                    var query_invln_contactid = contact.Id.ToString();
                    query.Criteria.AddCondition(invln_scheme.Fields.invln_contactid, ConditionOperator.Equal, query_invln_contactid);
                }

                if (contactWebRole != invln_Permission.Limiteduser)
                {
                    var query_invln_organisationid = organisationGuid.ToString();
                    query.Criteria.AddCondition(invln_scheme.Fields.invln_organisationid, ConditionOperator.Equal, query_invln_organisationid);

                }
            }
            else if (isLeadPartner == false && IsSitePartner == false)
            {
                var query_Or_invln_developingpartner = organisationGuid.ToString();
                var query_Or_invln_owneroftheland = organisationGuid.ToString();
                var query_Or_invln_ownerofthehomes = organisationGuid.ToString();

                var query_Or = new FilterExpression(LogicalOperator.Or);
                query.Criteria.AddFilter(query_Or);
                query_Or.AddCondition(invln_scheme.Fields.invln_DevelopingPartner, ConditionOperator.Equal, query_Or_invln_developingpartner);
                query_Or.AddCondition(invln_scheme.Fields.invln_OwneroftheLand, ConditionOperator.Equal, query_Or_invln_owneroftheland);
                query_Or.AddCondition(invln_scheme.Fields.invln_OwneroftheHomes, ConditionOperator.Equal, query_Or_invln_ownerofthehomes);
            }

            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_scheme>()).ToList();
        }

        public List<invln_scheme> GetByConsortiumId(Guid consortiumId)
        {
            var query = new QueryExpression(invln_scheme.EntityLogicalName);
            query.ColumnSet = new ColumnSet(invln_scheme.Fields.invln_DevelopingPartner,
                invln_scheme.Fields.invln_OwneroftheHomes,
                invln_scheme.Fields.invln_OwneroftheLand);
            var query_invln_sites = query.AddLink(
                invln_Sites.EntityLogicalName,
                invln_scheme.Fields.invln_Site,
                invln_Sites.Fields.invln_SitesId);
            var query_invln_sites_invln_ahpproject = query_invln_sites.AddLink(
                invln_ahpproject.EntityLogicalName,
                invln_Sites.Fields.invln_AHPProjectId,
                invln_ahpproject.Fields.invln_ahpprojectId);

            query_invln_sites_invln_ahpproject.LinkCriteria.AddCondition(invln_ahpproject.Fields.invln_ConsortiumId, ConditionOperator.Equal, consortiumId);

            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_scheme>()).ToList();
        }


        public List<invln_scheme> GetListOfApplicationToSendReminder(string calculatedDate)
        {
            var query_invln_lastexternalmodificationon = calculatedDate;
            var query_invln_stopreminderemail = false;
            var query_Or_invln_lastemailsenton = calculatedDate;

            var query = new QueryExpression(invln_scheme.EntityLogicalName);
            query.ColumnSet.AddColumns(invln_scheme.Fields.invln_schemeId);
            query.Criteria.AddCondition(invln_scheme.Fields.invln_lastexternalmodificationon, ConditionOperator.OnOrBefore, query_invln_lastexternalmodificationon);
            query.Criteria.AddCondition(invln_scheme.Fields.StatusCode, ConditionOperator.Equal, (int)invln_scheme_StatusCode.ReferredBackToApplicant);
            query.Criteria.AddCondition(invln_scheme.Fields.invln_stopreminderemail, ConditionOperator.Equal, query_invln_stopreminderemail);
            var query_Or = new FilterExpression(LogicalOperator.Or);
            query.Criteria.AddFilter(query_Or);
            query_Or.AddCondition(invln_scheme.Fields.invln_lastemailsenton, ConditionOperator.Null);
            query_Or.AddCondition(invln_scheme.Fields.invln_lastemailsenton, ConditionOperator.OnOrBefore, query_Or_invln_lastemailsenton);

            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_scheme>()).ToList();
        }


        public invln_scheme GetAllocation(Guid allocationId, Guid organisationId, Contact contact = null)
        {
            invln_scheme allocation = null;

            var query_invln_isallocation = true;
            var query_invln_schemeid = allocationId.ToString();
            var query_invln_organisationid = organisationId.ToString();

            var query = new QueryExpression(invln_scheme.EntityLogicalName);
            query.ColumnSet.AddColumns(
                invln_scheme.Fields.invln_schemeId,
                invln_scheme.Fields.invln_applicationid,
                invln_scheme.Fields.invln_schemename,
                invln_scheme.Fields.invln_HELocalAuthorityID,
                invln_scheme.Fields.invln_programmelookup,
                invln_scheme.Fields.invln_Tenure,
                invln_scheme.Fields.invln_TotalGrantAllocated,
                invln_scheme.Fields.invln_AmountPaid,
                invln_scheme.Fields.invln_AmountRemaining);
            query.Criteria.AddCondition(invln_scheme.Fields.invln_organisationid, ConditionOperator.Equal, query_invln_organisationid);
            query.Criteria.AddCondition(invln_scheme.Fields.invln_isallocation, ConditionOperator.Equal, query_invln_isallocation);
            query.Criteria.AddCondition(invln_scheme.Fields.invln_schemeId, ConditionOperator.Equal, query_invln_schemeid);

            if (contact != null)
            {
                var query_invln_contactid = contact.Id.ToString();
                query.Criteria.AddCondition(invln_scheme.Fields.invln_contactid, ConditionOperator.Equal, query_invln_contactid);
            }

            allocation = service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            return allocation;
        }

        public EntityCollection GetAllocationWithDeliveryPhaseAndClaims(string externalContactId, Guid accountId, Guid allocationId, Guid deliveryPhaseId)
        {
            var query_invln_schemeid = allocationId.ToString();
            var query_invln_organisationid = accountId.ToString();
            var deliveryPhase_invln_deliveryphaseid = deliveryPhaseId.ToString();
            var con_invln_externalid = externalContactId;

            var query = new QueryExpression(invln_scheme.EntityLogicalName);
            query.ColumnSet.AddColumns(
                invln_scheme.Fields.invln_schemeId,
                invln_scheme.Fields.invln_contactid,
                invln_scheme.Fields.invln_organisationid);
            query.Criteria.AddCondition(invln_scheme.Fields.invln_schemeId, ConditionOperator.Equal, query_invln_schemeid);
            query.Criteria.AddCondition(invln_scheme.Fields.invln_organisationid, ConditionOperator.Equal, query_invln_organisationid);
            var DeliveryPhase = query.AddLink(
                invln_DeliveryPhase.EntityLogicalName,
                invln_scheme.Fields.invln_schemeId,
                invln_DeliveryPhase.Fields.invln_Application);
            DeliveryPhase.EntityAlias = "DeliveryPhase";
            DeliveryPhase.Columns.AddColumns(
                invln_DeliveryPhase.Fields.invln_phasename,
                invln_DeliveryPhase.Fields.invln_DeliveryPhaseId,
                invln_DeliveryPhase.Fields.invln_Application,
                invln_DeliveryPhase.Fields.invln_NoofHomes,
                invln_DeliveryPhase.Fields.invln_nbrh,
                invln_DeliveryPhase.Fields.invln_rehabactivitytype,
                invln_DeliveryPhase.Fields.invln_buildactivitytype,


                invln_DeliveryPhase.Fields.invln_acquisitiondate,
                invln_DeliveryPhase.Fields.invln_AcquisitionValue,
                invln_DeliveryPhase.Fields.invln_AcquisitionPercentageValue,
                invln_DeliveryPhase.Fields.invln_acquisitionmilestoneclaimdate,

                invln_DeliveryPhase.Fields.invln_startonsitedate,
                invln_DeliveryPhase.Fields.invln_StartOnSiteValue,
                invln_DeliveryPhase.Fields.invln_StartOnSitePercentageValue,
                invln_DeliveryPhase.Fields.invln_startonsitemilestoneclaimdate,

                invln_DeliveryPhase.Fields.invln_completiondate,
                invln_DeliveryPhase.Fields.invln_CompletionValue,
                invln_DeliveryPhase.Fields.invln_CompletionPercentageValue,
                invln_DeliveryPhase.Fields.invln_completionmilestoneclaimdate

                );
            var ClaimAcquisition = DeliveryPhase.AddLink(
                invln_Claim.EntityLogicalName,
                invln_DeliveryPhase.Fields.invln_DeliveryPhaseId,
                invln_Claim.Fields.invln_DeliveryPhase,
                JoinOperator.LeftOuter);
            ClaimAcquisition.EntityAlias = "ClaimAcquisition";
            ClaimAcquisition.Columns.AddColumns(
                invln_Claim.Fields.invln_ClaimId,
                invln_Claim.Fields.invln_Milestone,
                invln_Claim.Fields.invln_Name,
                invln_Claim.Fields.StatusCode,
                invln_Claim.Fields.invln_AmountApportionedtoMilestone,
                invln_Claim.Fields.invln_PercentageofGrantApportionedtoThisMilestone,
                invln_Claim.Fields.invln_MilestoneDate,
                invln_Claim.Fields.invln_ClaimSubmissionDate,
                invln_Claim.Fields.invln_IncurredCosts,
                invln_Claim.Fields.invln_RequirementsConfirmation,
                invln_Claim.Fields.invln_ExternalStatus);
            ClaimAcquisition.LinkCriteria.AddCondition(invln_Claim.Fields.invln_Milestone, ConditionOperator.Equal, (int)invln_Milestone.Acquisition);
            var ClaimSoS = DeliveryPhase.AddLink(
                invln_Claim.EntityLogicalName,
                invln_DeliveryPhase.Fields.invln_DeliveryPhaseId,
                invln_Claim.Fields.invln_DeliveryPhase,
                JoinOperator.LeftOuter);
            ClaimSoS.EntityAlias = "ClaimSoS";
            ClaimSoS.Columns.AddColumns(
                invln_Claim.Fields.invln_ClaimId,
                invln_Claim.Fields.invln_Milestone,
                invln_Claim.Fields.invln_Name,
                invln_Claim.Fields.StatusCode,
                invln_Claim.Fields.invln_AmountApportionedtoMilestone,
                invln_Claim.Fields.invln_PercentageofGrantApportionedtoThisMilestone,
                invln_Claim.Fields.invln_MilestoneDate,
                invln_Claim.Fields.invln_ClaimSubmissionDate,
                invln_Claim.Fields.invln_IncurredCosts,
                invln_Claim.Fields.invln_RequirementsConfirmation,
                invln_Claim.Fields.invln_ExternalStatus);
            ClaimSoS.LinkCriteria.AddCondition(invln_Claim.Fields.invln_Milestone, ConditionOperator.Equal, (int)invln_Milestone.SoS);
            var ClaimPC = DeliveryPhase.AddLink(
                invln_Claim.EntityLogicalName,
                invln_DeliveryPhase.Fields.invln_DeliveryPhaseId,
                invln_Claim.Fields.invln_DeliveryPhase,
                JoinOperator.LeftOuter);
            ClaimPC.EntityAlias = "ClaimPC";
            ClaimPC.Columns.AddColumns(
                invln_Claim.Fields.invln_ClaimId,
                invln_Claim.Fields.invln_Milestone,
                invln_Claim.Fields.invln_Name,
                invln_Claim.Fields.StatusCode,
                invln_Claim.Fields.invln_AmountApportionedtoMilestone,
                invln_Claim.Fields.invln_PercentageofGrantApportionedtoThisMilestone,
                invln_Claim.Fields.invln_MilestoneDate,
                invln_Claim.Fields.invln_ClaimSubmissionDate,
                invln_Claim.Fields.invln_IncurredCosts,
                invln_Claim.Fields.invln_RequirementsConfirmation,
                invln_Claim.Fields.invln_ExternalStatus);
            ClaimPC.LinkCriteria.AddCondition(invln_Claim.Fields.invln_Milestone, ConditionOperator.Equal, (int)invln_Milestone.PC);
            var con = query.AddLink(Contact.EntityLogicalName, invln_scheme.Fields.invln_contactid, Contact.Fields.ContactId);
            con.EntityAlias = "con";

            con.LinkCriteria.AddCondition(Contact.Fields.invln_externalid, ConditionOperator.Equal, con_invln_externalid);

            if (deliveryPhaseId != Guid.Empty)
            {
                DeliveryPhase.LinkCriteria.AddCondition(invln_DeliveryPhase.Fields.invln_DeliveryPhaseId, ConditionOperator.Equal, deliveryPhase_invln_deliveryphaseid);
            }

            return service.RetrieveMultiple(query);
        }


    }
}
