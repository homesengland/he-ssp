using DataverseModel;
using HE.Base.Plugins.Attributes;
using HE.Base.Plugins.Common.Constants;
using HE.Base.Plugins.Handlers;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Handlers.ContactWebrole
{
    [CrmMessage(CrmMessage.Create)]
    [CrmProcessingStage(CrmProcessingStepStages.Postoperation)]
    public class PopulateParentCustomerOnContactHandler : CrmEntityHandlerBase<invln_contactwebrole, DataverseContext>
    {
        public override bool CanWork()
        {
            return ExecutionData.Target.invln_Accountid != null
                && ExecutionData.Target.invln_Contactid != null;
        }

        public override void DoWork()
        {
            var contactRepository = CrmRepositoriesFactory.GetBase<Contact, DataverseContext>();

            var contactColumns = new string[] { Contact.Fields.ParentCustomerId };
            var contact = contactRepository.GetById(ExecutionData.Target.invln_Contactid, contactColumns);
            if (contact.ParentCustomerId != null)
            {
                Logger.Trace("Contact has ParentCustomerId, skip execution");
                return;
            }

            contactRepository.Update(new Contact()
            {
                Id = contact.Id,
                ParentCustomerId = new EntityReference(Account.EntityLogicalName, ExecutionData.Target.invln_Accountid.Id)
            });
            Logger.Trace("Parent Customer Id has been updated");
        }
    }
}
