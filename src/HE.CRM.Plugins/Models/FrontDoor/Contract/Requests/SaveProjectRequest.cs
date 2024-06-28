using System;

namespace HE.CRM.Plugins.Models.FrontDoor.Contract.Requests
{
    internal sealed class SaveProjectRequest
    {
        public Guid? ProjectRecordId { get; set; }

        public string ProjectName { get; set; }

        public Guid OrganisationId { get; set; }

        public Guid? PortalOwnerId { get; set; }

        public int? ProjectType => 134370001; // Site

        public bool? ProjectSupportsHousingDeliveryInEngland { get; set; }

        public int[] ActivitiesInThisProject { get; set; }

        public int[] InfrastructureDelivered { get; set; }

        public int? AmountOfAffordableHomes { get; set; }

        public int? PreviousResidentialBuildingExperience { get; set; }

        public bool? IdentifiedSite { get; set; }

        public int? GeographicFocus { get; set; }

        public int[] Region { get; set; }

        public string LocalAuthority { get; set; }

        public int? NumberOfHomesEnabledBuilt { get; set; }

        public bool? WouldYourProjectFailWithoutHeSupport { get; set; }

        public bool? FundingRequired { get; set; }

        public int? AmountOfFundingRequired { get; set; }

        public bool? IntentionToMakeAProfit { get; set; }

        public int? StartOfProjectMonth { get; set; }

        public int? StartOfProjectYear { get; set; }

        public int[] FrontDoorDecision { get; set; }

        public int[] ProposedInterventions { get; set; }

        public int? LeadDirectorate { get; set; }
    }
}
