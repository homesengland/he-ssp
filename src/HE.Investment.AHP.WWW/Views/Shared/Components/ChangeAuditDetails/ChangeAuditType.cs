using System.ComponentModel;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ChangeAuditDetails;

public enum ChangeAuditType
{
    [Description("Last saved")]
    Modification,

    [Description("Submitted")]
    Submission,
}
