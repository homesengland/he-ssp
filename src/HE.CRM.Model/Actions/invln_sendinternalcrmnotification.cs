#pragma warning disable CS1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataverseModel
{
	
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_sendinternalcrmnotification")]
	public partial class invln_sendinternalcrmnotificationRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_notificationbody = "invln_notificationbody";
			public const string invln_notificationowner = "invln_notificationowner";
			public const string invln_notificationtitle = "invln_notificationtitle";
		}
		
		public const string ActionLogicalName = "invln_sendinternalcrmnotification";
		
		public string invln_notificationbody
		{
			get
			{
				if (this.Parameters.Contains("invln_notificationbody"))
				{
					return ((string)(this.Parameters["invln_notificationbody"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_notificationbody"] = value;
			}
		}
		
		public string invln_notificationowner
		{
			get
			{
				if (this.Parameters.Contains("invln_notificationowner"))
				{
					return ((string)(this.Parameters["invln_notificationowner"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_notificationowner"] = value;
			}
		}
		
		public string invln_notificationtitle
		{
			get
			{
				if (this.Parameters.Contains("invln_notificationtitle"))
				{
					return ((string)(this.Parameters["invln_notificationtitle"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_notificationtitle"] = value;
			}
		}
		
		public invln_sendinternalcrmnotificationRequest()
		{
			this.RequestName = "invln_sendinternalcrmnotification";
			this.invln_notificationbody = default(string);
			this.invln_notificationowner = default(string);
			this.invln_notificationtitle = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_sendinternalcrmnotification")]
	public partial class invln_sendinternalcrmnotificationResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public const string ActionLogicalName = "invln_sendinternalcrmnotification";
		
		public invln_sendinternalcrmnotificationResponse()
		{
		}
	}
}
#pragma warning restore CS1591
