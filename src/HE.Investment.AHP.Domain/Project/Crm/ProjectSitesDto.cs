using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Project.Crm;

public class ProjectSitesDto
{
    public string ProjectId { get; set; }

    public string ProjectName { get; set; }

    public IList<SiteDto> Sites { get; set; }
}
