using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Site;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Site
{
    public class CheckIfSiteWithGivenNameExistsHandler : CrmActionHandlerBase<invln_checkifsitewithgivennameexistsRequest, DataverseContext>
    {
        private string Name => ExecutionData.GetInputParameter<string>(invln_checkifsitewithgivennameexistsRequest.Fields.invln_sitename);

        public override bool CanWork()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        public override void DoWork()
        {
            var exist = CrmServicesFactory.Get<ISiteService>().Exist(Name);
            ExecutionData.SetOutputParameter(invln_checkifsitewithgivennameexistsResponse.Fields.invln_siteexists, exist);
        }
    }
}
