using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.QueryHandlers;

public record GetProjectDetailsQuery(LoanApplicationId LoanApplicationId, ProjectId ProjectId, ProjectFieldsSet ProjectFieldsSet) : IRequest<Project>;
