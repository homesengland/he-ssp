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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getsinglesite")]
	public partial class invln_getsinglesiteRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_accountid = "invln_accountid";
			public const string invln_externalcontactid = "invln_externalcontactid";
			public const string invln_siteid = "invln_siteid";
			public const string invln_fieldstoretrieve = "invln_fieldstoretrieve";
		}
		
		public const string ActionLogicalName = "invln_getsinglesite";
		
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
		
		public string invln_externalcontactid
		{
			get
			{
				if (this.Parameters.Contains("invln_externalcontactid"))
				{
					return ((string)(this.Parameters["invln_externalcontactid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_externalcontactid"] = value;
			}
		}
		
		public string invln_siteid
		{
			get
			{
				if (this.Parameters.Contains("invln_siteid"))
				{
					return ((string)(this.Parameters["invln_siteid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_siteid"] = value;
			}
		}
		
		public string invln_fieldstoretrieve
		{
			get
			{
				if (this.Parameters.Contains("invln_fieldstoretrieve"))
				{
					return ((string)(this.Parameters["invln_fieldstoretrieve"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_fieldstoretrieve"] = value;
			}
		}
		
		public invln_getsinglesiteRequest()
		{
			this.RequestName = "invln_getsinglesite";
			this.invln_siteid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getsinglesite")]
	public partial class invln_getsinglesiteResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_site = "invln_site";
		}
		
		public const string ActionLogicalName = "invln_getsinglesite";
		
		public invln_getsinglesiteResponse()
		{
		}
		
		public string invln_site
		{
			get
			{
				if (this.Results.Contains("invln_site"))
				{
					return ((string)(this.Results["invln_site"]));
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
