namespace HE.Common.IntegrationModel.PortalIntegrationModel;

public class ProjectWithAllocationListDto
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    public List<AllocationDto> ListOfAllocations { get; set; }
}
