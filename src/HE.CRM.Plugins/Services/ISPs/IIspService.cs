using DataverseModel;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.ISPs
{
    public interface IIspService : ICrmService
    {
        void SetFieldsOnSentForApprovalChange(invln_ISP target);
        void PopulateFieldsOnCreate(invln_ISP target);
        void CreateProjectConditionRecordsForIsp(invln_ISP target);
    }
}
