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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getconsortium")]
	public partial class invln_getconsortiumRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_consortiumid = "invln_consortiumid";
			public const string invln_memberorganisationid = "invln_memberorganisationid";
		}
		
		public const string ActionLogicalName = "invln_getconsortium";
		
		public string invln_consortiumid
		{
			get
			{
				if (this.Parameters.Contains("invln_consortiumid"))
				{
					return ((string)(this.Parameters["invln_consortiumid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_consortiumid"] = value;
			}
		}
		
		public string invln_memberorganisationid
		{
			get
			{
				if (this.Parameters.Contains("invln_memberorganisationid"))
				{
					return ((string)(this.Parameters["invln_memberorganisationid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_memberorganisationid"] = value;
			}
		}
		
		public invln_getconsortiumRequest()
		{
			this.RequestName = "invln_getconsortium";
			this.invln_consortiumid = default(string);
			this.invln_memberorganisationid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getconsortium")]
	public partial class invln_getconsortiumResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_consortium = "invln_consortium";
		}
		
		public const string ActionLogicalName = "invln_getconsortium";
		
		public invln_getconsortiumResponse()
		{
		}
		
		public string invln_consortium
		{
			get
			{
				if (this.Results.Contains("invln_consortium"))
				{
					return ((string)(this.Results["invln_consortium"]));
				}
				else
				{
					return default(string);
				}
			}
		}
	}
}
#pragma warning restore CS1591