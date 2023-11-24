using HE.Investments.Common.Domain;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;
using ApplicationStatus = HE.Investments.Loans.Contract.Application.Enums.ApplicationStatus;

namespace HE.Investments.Loans.Contract.Application.Queries;

public record GetTaskListDataQuery(LoanApplicationId Id) : IRequest<GetTaskListDataQueryResponse>;

public record GetTaskListDataQueryResponse(LoanApplicationId Id, LoanApplicationName ApplicationName, string OrganisationName, ApplicationStatus Status,
    bool CanBeSubmitted, bool WasSubmitted, Sections Sections, DateTime LastSavedOn, DateTime SubmittedOn, string LastModifiedBy);

public record Sections(SectionStatus CompanyStructure, SectionStatus Funding, SectionStatus Security, ProjectSection[] Projects);

public record ProjectSection(Guid? Id, string? Name, SectionStatus Status);
