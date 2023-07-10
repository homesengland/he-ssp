using HE.InvestmentLoans.BusinessLogic.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Application.Repositories
{
    internal class ApplicationProjectsRepository : IApplicationProjectsRepository
    {
        public List<SiteViewModel> GetAllProjects(Guid applicationId)
        {
            throw new NotImplementedException();
        }
    }
}
