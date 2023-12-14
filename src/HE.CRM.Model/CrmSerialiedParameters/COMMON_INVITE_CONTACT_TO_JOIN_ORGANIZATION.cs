using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION
    {
        public string templateId { get; set; }
        public parameters_COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION personalisation { get; set; }

    }

    public class parameters_COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string invitername { get; set; }
        public string organisationname { get; set; }
    }
}
