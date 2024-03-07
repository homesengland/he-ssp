using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Loans.Contract.CompanyStructure.Events;

public record FilesUploadedSuccessfullyEvent(IReadOnlyCollection<string> FileNames) : IDomainEvent;
