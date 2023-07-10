using HE.InvestmentLoans.BusinessLogic.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Project
{
    public interface IProjectContext
    {
        public string SelectedProjectId { get; }
        public string ApplicationId { get; }

        public Task<ProjectEntity> GetProjectDetails();

    }
}
