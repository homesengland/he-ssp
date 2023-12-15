using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class COMMON_REQUEST_TO_ASSIGN_CONTACT_TO_EXISTING_ORGANISATION
    {
        public string templateId { get; set; }
        public parameters_COMMON_REQUEST_TO_ASSIGN_CONTACT_TO_EXISTING_ORGANISATION personalisation { get; set; }

    }

    public class parameters_COMMON_REQUEST_TO_ASSIGN_CONTACT_TO_EXISTING_ORGANISATION
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string associatinguser { get; set; }
    }
}
