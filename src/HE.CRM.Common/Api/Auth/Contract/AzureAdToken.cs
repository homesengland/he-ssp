using System.Runtime.Serialization;

namespace HE.CRM.Common.Api.Auth.Contract
{
    [DataContract]
    internal sealed class AzureAdToken
    {
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "expires_in")]
        public string ExpiresIn { get; set; }

        [DataMember(Name = "ext_expires_in")]
        public string ExtExpiresIn { get; set; }

        [DataMember(Name = "expires_on")]
        public string ExpiresOn { get; set; }

        [DataMember(Name = "not_before")]
        public string NotBefore { get; set; }

        [DataMember(Name = "resource")]
        public string Resource { get; set; }

        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
    }
}
