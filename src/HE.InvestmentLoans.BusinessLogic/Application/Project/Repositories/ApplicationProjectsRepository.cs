using HE.InvestmentLoans.BusinessLogic.LoanApplication.Extensions;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.Application.Project.Repositories;

public class ApplicationProjectsRepository : IApplicationProjectsRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationProjectsRepository(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public LoanApplicationViewModel DeleteProject(Guid loanApplicationId, Guid projectId)
    {
        var loanApplicationSessionModel = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(loanApplicationId.ToString());

        var projectToDelete = loanApplicationSessionModel?.Sites.FirstOrDefault(p => p.Id == projectId) ??
            throw new NotFoundException(nameof(SiteViewModel), projectId);

        loanApplicationSessionModel!.Timestamp = DateTime.Now;
        loanApplicationSessionModel!.Sites.Remove(projectToDelete);

        _httpContextAccessor.HttpContext?.Session.Set(loanApplicationId.ToString(), loanApplicationSessionModel);

        return loanApplicationSessionModel;
    }
}
