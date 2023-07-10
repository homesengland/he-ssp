using HE.InvestmentLoans.BusinessLogic.ViewModel;
using System;

namespace HE.InvestmentLoans.BusinessLogic.Project.Repositories
{
    public interface IProjectRepository
    {
        public ProjectEntity AddProject(Guid applicationId, Guid projectId);
        public ProjectEntity UpdateProject(Guid applicationId, Guid projectId);
        public void DeleteProject(Guid applicationId, Guid projectId);
    }
}
