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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_sethometype")]
	public partial class invln_sethometypeRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_applicationid = "invln_applicationid";
			public const string invln_hometype = "invln_hometype";
			public const string invln_fieldstoset = "invln_fieldstoset";
			public const string invln_userid = "invln_userid";
			public const string invln_organisationid = "invln_organisationid";
		}
		
		public const string ActionLogicalName = "invln_sethometype";
		
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
		
		public string invln_hometype
		{
			get
			{
				if (this.Parameters.Contains("invln_hometype"))
				{
					return ((string)(this.Parameters["invln_hometype"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_hometype"] = value;
			}
		}
		
		public string invln_fieldstoset
		{
			get
			{
				if (this.Parameters.Contains("invln_fieldstoset"))
				{
					return ((string)(this.Parameters["invln_fieldstoset"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_fieldstoset"] = value;
			}
		}
		
		public string invln_userid
		{
			get
			{
				if (this.Parameters.Contains("invln_userid"))
				{
					return ((string)(this.Parameters["invln_userid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_userid"] = value;
			}
		}
		
		public string invln_organisationid
		{
			get
			{
				if (this.Parameters.Contains("invln_organisationid"))
				{
					return ((string)(this.Parameters["invln_organisationid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_organisationid"] = value;
			}
		}
		
		public invln_sethometypeRequest()
		{
			this.RequestName = "invln_sethometype";
			this.invln_applicationid = default(string);
			this.invln_hometype = default(string);
			this.invln_userid = default(string);
			this.invln_organisationid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_sethometype")]
	public partial class invln_sethometypeResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_hometypeid = "invln_hometypeid";
		}
		
		public const string ActionLogicalName = "invln_sethometype";
		
		public invln_sethometypeResponse()
		{
		}
		
		public string invln_hometypeid
		{
			get
			{
				if (this.Results.Contains("invln_hometypeid"))
				{
					return ((string)(this.Results["invln_hometypeid"]));
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
