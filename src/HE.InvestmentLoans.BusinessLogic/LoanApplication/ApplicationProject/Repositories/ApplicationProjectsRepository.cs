using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Extensions;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;

// THIS WHOLE CLASS IS NOT YET STARTED !!
public class ApplicationProjectsRepository : IApplicationProjectsRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDateTimeProvider _dateTime;

    public ApplicationProjectsRepository(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTime)
    {
        _httpContextAccessor = httpContextAccessor;
        _dateTime = dateTime;
    }

    public void AddAnotherProject(LoanApplicationEntity loanApplicationEntity, UserAccount userAccount)
    {
        var loanApplicationSessionModel = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(loanApplicationEntity.Id.ToString());

        loanApplicationSessionModel.Projects.Add(new Project());
        loanApplicationSessionModel.Timestamp = DateTime.Now;

        _httpContextAccessor.HttpContext?.Session.Set(loanApplicationEntity.Id.ToString(), loanApplicationSessionModel);
    }

    public void UpdateProject(LoanApplicationEntity loanApplicationEntity, Project project, UserAccount userAccount)
    {
        var loanApplicationSessionModel = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(loanApplicationEntity.Id.ToString());
        var projectToUpdate = loanApplicationSessionModel.Projects.FirstOrDefault(p => p.Id == project.Id);

        if (projectToUpdate == null)
        {
            // throw some error
        }

        loanApplicationSessionModel.Timestamp = DateTime.Now;

        // UPDATE PROJECT            

        //if (loanApplication.TryUpdateModelAction != null)
        //{
        //    _ = await request.TryUpdateModelAction(request.Model);
        //}

        _httpContextAccessor.HttpContext?.Session.Set(loanApplicationEntity.Id.ToString(), loanApplicationSessionModel);
    }

    public void DeleteProject(LoanApplicationEntity loanApplicationEntity, Guid projectId, UserAccount userAccount)
    {
        var loanApplicationSessionModel = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(loanApplicationEntity.Id.ToString())!;

        var projectToDelete = loanApplicationSessionModel.Sites.FirstOrDefault(p => p.Id == projectId) ?? throw new NotFoundException(nameof(SiteViewModel).ToString(), projectId);

        loanApplicationSessionModel!.Timestamp = _dateTime.Now;
        loanApplicationSessionModel!.Sites.Remove(projectToDelete);

        _httpContextAccessor.HttpContext?.Session.Set(loanApplicationEntity.Id.ToString(), loanApplicationSessionModel);
    }

    public ApplicationProjects GetAllProjects(LoanApplicationId loanApplicationId, UserAccount userAccount)
    {
        var loanApplicationSessionModel = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(loanApplicationId.ToString());

        if (loanApplicationSessionModel == null)
        {
            // throw error
        }

        var result = loanApplicationSessionModel.Projects.ToList();

        return new ApplicationProjects(loanApplicationId);
    }

    public Project GetProjectDetails(LoanApplicationId loanApplicationId, Guid projectId, UserAccount userAccount)
    {
        var loanApplicationSessionModel = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(loanApplicationId.ToString());

        if (loanApplicationSessionModel == null)
        {
            // throw error
        }

        var result = loanApplicationSessionModel.Projects.FirstOrDefault(p => p.Id == projectId);

        return result;
    }
}
