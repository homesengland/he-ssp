using System;

namespace HE.CRM.Common.Api.FrontDoor.Contract.Responses
{
    public sealed class GetProjectResponse
    {
        public Guid? ProjectId { get; set; }

        public string LocalAuthorityName { get; set; }

        public string LocalAuthorityCode { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }

        public FrontDoorProjectContact FrontDoorProjectContact { get; set; }

        public int? RecordStatus { get; set; }

        public Guid ProjectRecordId { get; set; }

        public Guid OrganisationId { get; set; }

        public Guid PortalOwnerId { get; set; }

        public string ProjectName { get; set; }

        public int? ProjectType { get; set; }

        public bool ProjectSupportsHousingDeliveryInEngland { get; set; }

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

        public int[] ProposedInterventions { get; set; }

        public int? LeadDirectorate { get; set; }

        public int[] FrontDoorDecision { get; set; }
    }
}
