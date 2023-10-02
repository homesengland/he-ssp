using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Queries;
public record GetProjectQuery(LoanApplicationId ApplicationId, ProjectId ProjectId) : IRequest<ProjectViewModel>;
