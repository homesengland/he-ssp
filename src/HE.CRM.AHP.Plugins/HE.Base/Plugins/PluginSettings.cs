using HE.Base.Log;
using System.Runtime.Serialization;

namespace HE.Base.Plugins
{
    [DataContract]
    public class PluginSettings
    {
        [DataMember]
        public LogSettings LogSettings { get; set; }
    }
}
