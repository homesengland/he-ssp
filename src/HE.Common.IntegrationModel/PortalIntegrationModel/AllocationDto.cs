using System;
using System.Collections.Generic;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class AllocationDto
    {
        public Guid Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string Name { get; set; }
        public LocalAuthorityDto LocalAuthority { get; set; }
        public Guid ProgrammeId { get; set; }
        public int Tenure { get; set; }
        public string FDProjectId { get; set; }
        public bool IsInContract { get; set; }
        public bool HasDraftAllocation { get; set;}
        public string OrganisationName { get; set; }
        public DateTime LastExternalModificationOn { get; set; }
        public ContactDto LastExternalModificationBy { get; set; }
    }
}
