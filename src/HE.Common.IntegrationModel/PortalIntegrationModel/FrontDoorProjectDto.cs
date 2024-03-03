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
        public List<int> ActivitiesinThisProject { get; set; } // choises multiselect
        public List<int> InfrastructureDelivered { get; set; } // choises multiselect
        public int? AmountofAffordableHomes { get; set; } //choises
        public int? PreviousResidentialBuildingExperience { get; set; }
        public bool? IdentifiedSite { get; set; }
        public int? GeographicFocus { get; set; }  //choises
        public List<int> Region { get; set; } // choises multiselect
        public Guid LocalAuthority { get; set; }
        public int? NumberofHomesEnabled_Built { get; set; }
        public bool? WouldyourprojectfailwithoutHEsupport { get; set; }
        public bool? FundingRequired { get; set; }
        public int? AmountofFundingRequired { get; set; } //choises
        public int? StartofProjectMonth { get; set; }
        public int? StartofProjectYear { get; set; }
        public UserAccountDto FrontDoorProjectContact { get; set; }
        public List<FrontDoorProjectSiteDto> FrontDoorSiteList { get; set; }
    }
}
