using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.Queries;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Projects.QueryHandlers;
public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ProjectViewModel>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly ILoanUserContext _loanUserContext;

    public GetProjectQueryHandler(IApplicationProjectsRepository applicationProjectsRepository, ILoanUserContext loanUserContext)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<ProjectViewModel> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        var applicationProjects = await _applicationProjectsRepository.GetById(request.ApplicationId, await _loanUserContext.GetSelectedAccount(), request.ProjectFieldsSet, cancellationToken);

        var project = applicationProjects.Projects.FirstOrDefault(c => c.Id == request.ProjectId)
            ?? throw new NotFoundException(nameof(Project), request.ProjectId.ToString());

        return ProjectMapper.MapToViewModel(project, applicationProjects.LoanApplicationId);
    }
}
