using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
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
        var applicationProjects = await _applicationProjectsRepository.GetById(request.ApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        var project = applicationProjects.Projects.FirstOrDefault(c => c.Id == request.ProjectId)
            ?? throw new NotFoundException(nameof(Project), request.ProjectId.ToString());

        return new ProjectViewModel
        {
            ProjectId = project.Id!.Value,
            Name = project.Name?.Value,
            ApplicationId = applicationProjects.LoanApplicationId.Value,
            PlanningReferenceNumberExists = project.PlanningReferenceNumber?.Exists.MapToCommonResponse(),
            PlanningReferenceNumber = project.PlanningReferenceNumber?.Value,
            LocationCoordinates = project.Coordinates?.Value,
            LocationLandRegistry = project.LandRegistryTitleNumber?.Value,
            Ownership = project.LandOwnership.IsProvided() ? project.LandOwnership.ApplicantHasFullOwnership.MapToCommonResponse() : null!,
        };
    }
}
