namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class PhaseClaimsDto
    {
        public string Id { get; set; }
        public string AllocationId { get; set; }
        public string Name { get; set; }
        public int NumberOfHomes { get; set; }
        public int? NewBuildActivityType { get; set; }
        public int? RehabBuildActivityType { get; set; }
        public MilestoneClaimDto AcquisitionMilestone { get; set; }
        public MilestoneClaimDto StartOnSiteMilestone { get; set; }
        public MilestoneClaimDto CompletionMilestone { get; set; }
    }
}
