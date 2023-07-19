using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Extensions;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;

public class ApplicationProjectsRepository : IApplicationProjectsRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDateTimeProvider _dateTime;

    public ApplicationProjectsRepository(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTime)
    {
        _httpContextAccessor = httpContextAccessor;
        _dateTime = dateTime;
    }

    public void Add(ApplicationProjects applicationProjects)
    {
        applicationProjects.AddAnotherProject();
    }

    public void Update(ApplicationProjects applicationProjects, Project project)
    {
        applicationProjects.UpdateProject(project);
    }

    public void Delete(ApplicationProjects applicationProjects, ProjectId projectId)
    {
        applicationProjects.DeleteProject(projectId);
    }

    public ApplicationProjects GetAll(LoanApplicationId loanApplicationId, UserAccount userAccount)
    {
        var loanApplication = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationEntity>(loanApplicationId.ToString())
            ?? throw new NotFoundException(nameof(LoanApplicationEntity).ToString(), loanApplicationId.ToString());

        return loanApplication.ApplicationProjects;
    }

    public Project GetById(LoanApplicationId loanApplicationId, ProjectId projectId, UserAccount userAccount)
    {
        var loanApplication = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationEntity>(loanApplicationId.ToString())
           ?? throw new NotFoundException(nameof(LoanApplicationEntity).ToString(), loanApplicationId.ToString());

        var result = loanApplication.ApplicationProjects.Projects.FirstOrDefault(p => p.Id == projectId)
            ?? throw new NotFoundException(nameof(Project).ToString(), projectId.ToString());

        return result;
    }

    public void Save(ApplicationProjects applicationProjects)
    {
        var loanApplication = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationEntity>(applicationProjects.LoanApplicationId.ToString())
           ?? throw new NotFoundException(nameof(LoanApplicationEntity).ToString(), applicationProjects.LoanApplicationId.ToString());

        loanApplication.SaveApplicationProjects(applicationProjects);

        loanApplication.LegacyModel.SetTimestamp(_dateTime.Now);

        _httpContextAccessor.HttpContext?.Session.Set(applicationProjects.LoanApplicationId.ToString(), loanApplication);
    }
}
