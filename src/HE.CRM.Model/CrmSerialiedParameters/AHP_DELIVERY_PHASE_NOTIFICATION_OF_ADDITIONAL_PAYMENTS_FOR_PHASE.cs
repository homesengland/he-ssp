using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADDITIONAL_PAYMENTS_FOR_PHASE
    {
        public string templateId { get; set; }
        public parameters_AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADDITIONAL_PAYMENTS_FOR_PHASE personalisation { get; set; }
    }


    public class parameters_AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADDITIONAL_PAYMENTS_FOR_PHASE
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string organisationname { get; set; }
    }
}

