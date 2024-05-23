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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_issiteorapplicationpartner")]
	public partial class invln_issiteorapplicationpartnerRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_consortiumid = "invln_consortiumid";
			public const string invln_organizationid = "invln_organizationid";
		}
		
		public const string ActionLogicalName = "invln_issiteorapplicationpartner";
		
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
		
		public string invln_organizationid
		{
			get
			{
				if (this.Parameters.Contains("invln_organizationid"))
				{
					return ((string)(this.Parameters["invln_organizationid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_organizationid"] = value;
			}
		}
		
		public invln_issiteorapplicationpartnerRequest()
		{
			this.RequestName = "invln_issiteorapplicationpartner";
			this.invln_consortiumid = default(string);
			this.invln_organizationid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_issiteorapplicationpartner")]
	public partial class invln_issiteorapplicationpartnerResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_consortiumpartnerstatus = "invln_consortiumpartnerstatus";
		}
		
		public const string ActionLogicalName = "invln_issiteorapplicationpartner";
		
		public invln_issiteorapplicationpartnerResponse()
		{
		}
		
		public int invln_consortiumpartnerstatus
		{
			get
			{
				if (this.Results.Contains("invln_consortiumpartnerstatus"))
				{
					return ((int)(this.Results["invln_consortiumpartnerstatus"]));
				}
				else
				{
					return default(int);
				}
			}
		}
	}
}
#pragma warning restore CS1591
