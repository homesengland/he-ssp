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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_changeloanapplicationexternalstatus")]
	public partial class invln_changeloanapplicationexternalstatusRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_changereason = "invln_changereason";
			public const string invln_loanapplicationid = "invln_loanapplicationid";
			public const string invln_statusexternal = "invln_statusexternal";
		}
		
		public const string ActionLogicalName = "invln_changeloanapplicationexternalstatus";
		
		public string invln_changereason
		{
			get
			{
				if (this.Parameters.Contains("invln_changereason"))
				{
					return ((string)(this.Parameters["invln_changereason"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_changereason"] = value;
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
		
		public int invln_statusexternal
		{
			get
			{
				if (this.Parameters.Contains("invln_statusexternal"))
				{
					return ((int)(this.Parameters["invln_statusexternal"]));
				}
				else
				{
					return default(int);
				}
			}
			set
			{
				this.Parameters["invln_statusexternal"] = value;
			}
		}
		
		public invln_changeloanapplicationexternalstatusRequest()
		{
			this.RequestName = "invln_changeloanapplicationexternalstatus";
			this.invln_changereason = default(string);
			this.invln_loanapplicationid = default(string);
			this.invln_statusexternal = default(int);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_changeloanapplicationexternalstatus")]
	public partial class invln_changeloanapplicationexternalstatusResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public const string ActionLogicalName = "invln_changeloanapplicationexternalstatus";
		
		public invln_changeloanapplicationexternalstatusResponse()
		{
		}
	}
}
#pragma warning restore CS1591
