using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HE.AZURE.CalculationAPI.DTO
{
    [DataContract]
    public class CriteriaDto
    {
        [Required]
        [DataMember(Name = "Alias")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "Value")]
        public string Value { get; set; }

        public CriteriaDto(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
