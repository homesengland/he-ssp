namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class AhpApplicationForSiteDto
    {
        public string applicationId { get; set; }
        public string applicationName { get; set; }
        public int? applicationStatus { get; set; }
        public string requiredFunding { get; set; }
        public int? housesToDeliver { get; set; }
        public int? tenure { get; set; }
    }
}
