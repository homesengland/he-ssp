using HE.InvestmentLoans.BusinessLogic._LoanApplication.Extensions;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace HE.InvestmentLoans.BusinessLogic.Application.Project.Repositories;

internal class ApplicationProjectsRepository : IApplicationProjectsRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDateTimeProvider _dateTime;

    public ApplicationProjectsRepository(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTime)
    {

        _httpContextAccessor = httpContextAccessor;
        _dateTime = dateTime;
    }

    public LoanApplicationViewModel DeleteProject(Guid loanApplicationId, Guid projectId)
    {
        var loanApplicationSessionModel = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(loanApplicationId.ToString());

        var projectToDelete = loanApplicationSessionModel.Sites.FirstOrDefault(p => p.Id == projectId);

        if (projectToDelete == null)
        {
            throw new NotFoundException(projectToDelete.GetType().ToString(), projectId);
        }

        loanApplicationSessionModel.Timestamp = _dateTime.Now;

        loanApplicationSessionModel.Sites.Remove(projectToDelete);

        _httpContextAccessor.HttpContext?.Session.Set(loanApplicationId.ToString(), loanApplicationSessionModel);

        return loanApplicationSessionModel;
    }
}
