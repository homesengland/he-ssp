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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_setconsortium")]
	public partial class invln_setconsortiumRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_userid = "invln_userid";
			public const string invln_programmeId = "invln_programmeId";
			public const string invln_consortiumname = "invln_consortiumname";
			public const string invln_leadpartner = "invln_leadpartner";
		}
		
		public const string ActionLogicalName = "invln_setconsortium";
		
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
		
		public string invln_programmeId
		{
			get
			{
				if (this.Parameters.Contains("invln_programmeId"))
				{
					return ((string)(this.Parameters["invln_programmeId"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_programmeId"] = value;
			}
		}
		
		public string invln_consortiumname
		{
			get
			{
				if (this.Parameters.Contains("invln_consortiumname"))
				{
					return ((string)(this.Parameters["invln_consortiumname"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_consortiumname"] = value;
			}
		}
		
		public string invln_leadpartner
		{
			get
			{
				if (this.Parameters.Contains("invln_leadpartner"))
				{
					return ((string)(this.Parameters["invln_leadpartner"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_leadpartner"] = value;
			}
		}
		
		public invln_setconsortiumRequest()
		{
			this.RequestName = "invln_setconsortium";
			this.invln_userid = default(string);
			this.invln_programmeId = default(string);
			this.invln_consortiumname = default(string);
			this.invln_leadpartner = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_setconsortium")]
	public partial class invln_setconsortiumResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_consortiumid = "invln_consortiumid";
		}
		
		public const string ActionLogicalName = "invln_setconsortium";
		
		public invln_setconsortiumResponse()
		{
		}
		
		public string invln_consortiumid
		{
			get
			{
				if (this.Results.Contains("invln_consortiumid"))
				{
					return ((string)(this.Results["invln_consortiumid"]));
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
