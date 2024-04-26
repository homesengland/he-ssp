using System.Collections.Generic;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class ConsortiumDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public string leadPartnerId { get; set; }
        public string leadPartnerName { get; set; }
        public List<ConsortiumMemberDto> members { get; set; }
        public string programmeId { get; set; }

    }
}
