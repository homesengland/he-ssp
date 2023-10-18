using System.ComponentModel;

namespace HE.InvestmentLoans.Contract.Application.Enums;

public enum SectionStatus
{
    Undefined,
    [Description("Not Started")]
    NotStarted,
    [Description("In Progress")]
    InProgress,
    Completed,
    Submitted,
    [Description("Not Submitted")]
    NotSubmitted,
    Withdrawn,
}
