using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    internal class AHP_EXTERNAL_APPLICATION_REFERRED_BACK_TO_APPLICANT
    {
        public string templateId { get; set; }
        public parameters_AHP_EXTERNAL_APPLICATION_REFERRED_BACK_TO_APPLICANT personalisation { get; set; }
    }
    public class parameters_AHP_EXTERNAL_APPLICATION_REFERRED_BACK_TO_APPLICANT
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string name { get; set; }
        public string schemename { get; set; }
        public string programmename { get; set; }
    }
}
