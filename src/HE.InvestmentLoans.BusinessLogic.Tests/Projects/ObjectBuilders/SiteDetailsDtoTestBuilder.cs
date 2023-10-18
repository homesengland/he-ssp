using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.CRM.Model;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
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
