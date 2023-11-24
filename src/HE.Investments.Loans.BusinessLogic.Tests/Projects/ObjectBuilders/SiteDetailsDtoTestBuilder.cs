using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
internal sealed class SiteDetailsDtoTestBuilder
{
    private readonly SiteDetailsDto _dto;

    public SiteDetailsDtoTestBuilder()
    {
        _dto = new SiteDetailsDto
        {
            siteDetailsId = ProjectIdTestData.AnyProjectId.ToString(),
        };
    }

    public static SiteDetailsDtoTestBuilder New() => new();

    public SiteDetailsDtoTestBuilder WithStartDate(bool? hasStartDate, DateTime? startDate = null)
    {
        _dto.projectHasStartDate = hasStartDate;
        _dto.startDate = startDate;

        return this;
    }

    public SiteDetailsDto Build() => _dto;
}
