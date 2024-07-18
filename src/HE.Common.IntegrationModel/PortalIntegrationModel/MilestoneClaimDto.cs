using System;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class MilestoneClaimDto
    {
        public int Type { get; set; }
        public int Status { get; set; }
        public decimal AmountOfGrantApportioned { get; set; }
        public decimal PercentageOfGrantApportioned { get; set; }
        public DateTime ForecastClaimDate { get; set; }
        public DateTime? AchievementDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public bool? CostIncurred { get; set; }
        public bool? IsConfirmed { get; set; }
    }
}
