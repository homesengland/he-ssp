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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_checkifsitewithgivennameexists")]
	public partial class invln_checkifsitewithgivennameexistsRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_sitename = "invln_sitename";
		}
		
		public const string ActionLogicalName = "invln_checkifsitewithgivennameexists";
		
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
		
		public invln_checkifsitewithgivennameexistsRequest()
		{
			this.RequestName = "invln_checkifsitewithgivennameexists";
			this.invln_sitename = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_checkifsitewithgivennameexists")]
	public partial class invln_checkifsitewithgivennameexistsResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_siteexists = "invln_siteexists";
		}
		
		public const string ActionLogicalName = "invln_checkifsitewithgivennameexists";
		
		public invln_checkifsitewithgivennameexistsResponse()
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
