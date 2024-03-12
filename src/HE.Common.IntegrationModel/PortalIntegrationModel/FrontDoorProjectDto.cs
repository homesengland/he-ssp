using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Xrm.Sdk;


namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class FrontDoorProjectDto
    {
        public string ProjectId { get; set; }
        public string externalId { get; set; }
        public Guid OrganisationId { get; set; }
        public string ProjectName { get; set; }
        public bool? ProjectSupportsHousingDeliveryinEngland { get; set; }
        public List<int> ActivitiesinThisProject { get; set; }
        public List<int> InfrastructureDelivered { get; set; }
        public int? AmountofAffordableHomes { get; set; }
        public int? PreviousResidentialBuildingExperience { get; set; }
        public bool? IdentifiedSite { get; set; }
        public int? GeographicFocus { get; set; }
        public List<int> Region { get; set; }
        public Guid LocalAuthority { get; set; }
        public int? NumberofHomesEnabledBuilt { get; set; }
        public bool? WouldyourprojectfailwithoutHEsupport { get; set; }
        public bool? FundingRequired { get; set; }
        public int? AmountofFundingRequired { get; set; }
        public int? StartofProjectMonth { get; set; }
        public int? StartofProjectYear { get; set; }
        public DateTime? CreatedOn { get; set; }
        public UserAccountDto FrontDoorProjectContact { get; set; }
        public bool? IntentiontoMakeaProfit { get; set; }
    } 
}
