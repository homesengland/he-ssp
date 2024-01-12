using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.CrmFields;

public static class ProgrammeFields
{
    public const string EntityName = "invln_programme";
    public const string Name = "invln_programmename";
    public const string StartDate = "invln_programmestartdate";
    public const string EndDate = "invln_programmeenddate";

    public static readonly ColumnSet Columns = new(Name, StartDate, EndDate);
}
