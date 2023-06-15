using System.Runtime.Serialization;

namespace HE.AZURE.CalculationAPI.DTO
{
    [DataContract]
    public class CalculateResponseDto
    {
        [DataMember(Name = "calculationResult")]
        public double? CalculationResult { get; set; }
    }
}
