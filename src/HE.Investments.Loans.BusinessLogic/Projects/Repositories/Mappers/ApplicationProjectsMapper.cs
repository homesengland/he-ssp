extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;

internal static class ApplicationProjectsMapper
{
    public static ApplicationProjects Map(LoanApplicationDto loanApplicationDto, DateTime now)
    {
        var loanApplicationId = LoanApplicationId.From(loanApplicationDto.loanApplicationId);
        var projects = loanApplicationDto.siteDetailsList.Select(ProjectEntityMapper.Map);

        return new ApplicationProjects(loanApplicationId, projects);
    }
}
