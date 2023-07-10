using HE.InvestmentLoans.BusinessLogic.ViewModel;
using System;
using System.Collections.Generic;

namespace HE.InvestmentLoans.BusinessLogic.Application.Repositories
{
    public interface IApplicationProjectsRepository
    {
        public List<SiteViewModel> GetAllProjects(Guid applicationId);
    }
}
