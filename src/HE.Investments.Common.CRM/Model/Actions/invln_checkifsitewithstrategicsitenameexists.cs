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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_checkifsitewithstrategicsitenameexists")]
	public partial class invln_checkifsitewithstrategicsitenameexistsRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_sitename = "invln_sitename";
			public const string invln_accountid = "invln_accountid";
		}
		
		public const string ActionLogicalName = "invln_checkifsitewithstrategicsitenameexists";
		
		public string invln_sitename
		{
			get
			{
				if (this.Parameters.Contains("invln_sitename"))
				{
					return ((string)(this.Parameters["invln_sitename"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_sitename"] = value;
			}
		}
		
		public string invln_accountid
		{
			get
			{
				if (this.Parameters.Contains("invln_accountid"))
				{
					return ((string)(this.Parameters["invln_accountid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_accountid"] = value;
			}
		}
		
		public invln_checkifsitewithstrategicsitenameexistsRequest()
		{
			this.RequestName = "invln_checkifsitewithstrategicsitenameexists";
			this.invln_sitename = default(string);
			this.invln_accountid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_checkifsitewithstrategicsitenameexists")]
	public partial class invln_checkifsitewithstrategicsitenameexistsResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_siteexists = "invln_siteexists";
		}
		
		public const string ActionLogicalName = "invln_checkifsitewithstrategicsitenameexists";
		
		public invln_checkifsitewithstrategicsitenameexistsResponse()
		{
		}
		
		public string invln_siteexists
		{
			get
			{
				if (this.Results.Contains("invln_siteexists"))
				{
					return ((string)(this.Results["invln_siteexists"]));
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