#pragma warning disable CS1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HE.Investments.Common.CRM.Model
{
	
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_sendreminderemailforrefferedbacktoapplicant")]
	public partial class invln_sendreminderemailforrefferedbacktoapplicantRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_applicationid = "invln_applicationid";
		}
		
		public const string ActionLogicalName = "invln_sendreminderemailforrefferedbacktoapplicant";
		
		public string invln_applicationid
		{
			get
			{
				if (this.Parameters.Contains("invln_applicationid"))
				{
					return ((string)(this.Parameters["invln_applicationid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_applicationid"] = value;
			}
		}
		
		public invln_sendreminderemailforrefferedbacktoapplicantRequest()
		{
			this.RequestName = "invln_sendreminderemailforrefferedbacktoapplicant";
			this.invln_applicationid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_sendreminderemailforrefferedbacktoapplicant")]
	public partial class invln_sendreminderemailforrefferedbacktoapplicantResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public const string ActionLogicalName = "invln_sendreminderemailforrefferedbacktoapplicant";
		
		public invln_sendreminderemailforrefferedbacktoapplicantResponse()
		{
		}
	}
}
#pragma warning restore CS1591
