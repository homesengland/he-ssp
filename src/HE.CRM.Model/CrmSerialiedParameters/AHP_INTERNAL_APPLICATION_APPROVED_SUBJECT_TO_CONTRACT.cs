using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class AHP_INTERNAL_APPLICATION_APPROVED_SUBJECT_TO_CONTRACT
    {
        public string templateId { get; set; }
        public parameters_AHP_INTERNAL_APPLICATION_APPROVED_SUBJECT_TO_CONTRACT personalisation { get; set; }
    }
    public class parameters_AHP_INTERNAL_APPLICATION_APPROVED_SUBJECT_TO_CONTRACT
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string name { get; set; }
        public string organisationname { get; set; }
        public string applicationname { get; set; }
        public string applicationurl { get; set; }
    }
}
