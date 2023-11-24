using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Queries;
public record GetProjectQuery(LoanApplicationId ApplicationId, ProjectId ProjectId, ProjectFieldsSet ProjectFieldsSet) : IRequest<ProjectViewModel>;
