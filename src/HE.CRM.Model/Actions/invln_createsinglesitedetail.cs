#pragma warning disable CS1591
// Code Generated by DLaB.ModelBuilderExtensions
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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_createsinglesitedetail")]
	public partial class invln_createsinglesitedetailRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_sitedetails = "invln_sitedetails";
			public const string invln_loanapplicationid = "invln_loanapplicationid";
		}
		
		public const string ActionLogicalName = "invln_createsinglesitedetail";
		
		public string invln_sitedetails
		{
			get
			{
				if (this.Parameters.Contains("invln_sitedetails"))
				{
					return ((string)(this.Parameters["invln_sitedetails"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_sitedetails"] = value;
			}
		}
		
		public string invln_loanapplicationid
		{
			get
			{
				if (this.Parameters.Contains("invln_loanapplicationid"))
				{
					return ((string)(this.Parameters["invln_loanapplicationid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_loanapplicationid"] = value;
			}
		}
		
		public invln_createsinglesitedetailRequest()
		{
			this.RequestName = "invln_createsinglesitedetail";
			this.invln_sitedetails = default(string);
			this.invln_loanapplicationid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_createsinglesitedetail")]
	public partial class invln_createsinglesitedetailResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public const string ActionLogicalName = "invln_createsinglesitedetail";
		
		public invln_createsinglesitedetailResponse()
		{
		}
	}
}
#pragma warning restore CS1591
