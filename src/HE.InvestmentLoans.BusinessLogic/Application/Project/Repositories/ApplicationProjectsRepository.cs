using HE.InvestmentLoans.BusinessLogic._LoanApplication.Extensions;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace HE.InvestmentLoans.BusinessLogic.Application.Project.Repositories;

internal class ApplicationProjectsRepository : IApplicationProjectsRepository
{
    private IHttpContextAccessor _httpContextAccessor;

    public ApplicationProjectsRepository(IHttpContextAccessor httpContextAccessor)
    {

        _httpContextAccessor = httpContextAccessor;
    }

    public LoanApplicationViewModel DeleteProject(Guid loanApplicationId, Guid projectId)
    {
        var loanApplicationSessionModel = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(loanApplicationId.ToString());

        var projectToDelete = loanApplicationSessionModel.Sites.Where(p => p.Id == projectId).FirstOrDefault();

        if (projectToDelete == null)
        {
            throw new DeleteFailureException(projectToDelete.GetType().ToString(), projectId, "Project doesn't exist.");
        }

        loanApplicationSessionModel.Timestamp = DateTime.Now;

        loanApplicationSessionModel.Sites.Remove(projectToDelete);

        _httpContextAccessor.HttpContext?.Session.Set(loanApplicationId.ToString(), loanApplicationSessionModel);

        return loanApplicationSessionModel;
    }
}
