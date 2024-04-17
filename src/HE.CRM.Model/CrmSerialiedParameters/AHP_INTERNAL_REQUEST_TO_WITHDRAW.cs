using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class AHP_INTERNAL_REQUEST_TO_WITHDRAW
    {
        public string templateId { get; set; }
        public parameters_AHP_INTERNAL_REQUEST_TO_WITHDRAW personalisation { get; set; }

    }


    public class parameters_AHP_INTERNAL_REQUEST_TO_WITHDRAW
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string organisationname { get; set; }
        public string reason { get; set; }
    }
}
