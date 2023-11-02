using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Application.Queries;

public record GetTaskListDataQuery(LoanApplicationId Id) : IRequest<GetTaskListDataQueryResponse>;

public record GetTaskListDataQueryResponse(LoanApplicationId Id, LoanApplicationName ApplicationName, string OrganisationName, ApplicationStatus Status, bool CanBeSubmitted, bool WasSubmitted, Sections Sections, DateTime DisplayDate, string LastModifiedBy);

public record Sections(SectionStatus CompanyStructure, SectionStatus Funding, SectionStatus Security, ProjectSection[] Projects);

public record ProjectSection(Guid? Id, string? Name, SectionStatus Status);
