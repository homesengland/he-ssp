namespace HE.CRM.Model.CrmSerialiedParameters
{
    public class AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADJUSTMENT_ACCEPTED
    {
        public string templateId { get; set; }
        public parameters_AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADJUSTMENT_ACCEPTED personalisation { get; set; }
    }

    public class parameters_AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADJUSTMENT_ACCEPTED
    {
        // Required always
        public string subject { get; set; }
        public string recipientEmail { get; set; }
        // Additional
        public string username { get; set; }
        public string organisationname { get; set; }
    }
}
