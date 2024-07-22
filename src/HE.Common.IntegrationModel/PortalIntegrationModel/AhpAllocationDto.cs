using System.Collections.Generic;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class AhpAllocationDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ReferenceNumber { get; set; }
        public LocalAuthorityDto LocalAuthority { get; set; }
        public string ProgrammeId { get; set; }
        public int Tenure { get; set; }
        public int Homes { get; set; }
    }
}
