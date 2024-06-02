using System;
using System.Collections.Generic;
using System.Text;


namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class AHP_EXTERNAL_APPLICATION_APPROVED_OUTCOME
    {
        public string templateId { get; set; }
        public parameters_AHP_EXTERNAL_APPLICATION_APPROVED_OUTCOME personalisation { get; set; }
    }

    public class parameters_AHP_EXTERNAL_APPLICATION_APPROVED_OUTCOME
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string name { get; set; }
        public string grantamount { get; set; }
        public string applicationname { get; set; }
        public string applicationid { get; set; }
        public string partnername { get; set; }
        public string tenure { get; set; }
        public string homesenglandfunding { get; set; }
        public string homes { get; set; }
        public string providermanagementlead { get; set; }
        public string allocationofgrant { get; set; }
    }
}
