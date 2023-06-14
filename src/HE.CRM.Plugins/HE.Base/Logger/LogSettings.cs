using System.Runtime.Serialization;

namespace HE.Base.Log
{
    [DataContract]
    public class LogSettings
    {
        [DataMember]
        public LogLevel Level { get; set; }
    }
}
