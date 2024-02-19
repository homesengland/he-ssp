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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getcontactrole")]
	public partial class invln_getcontactroleRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_contactemail = "invln_contactemail";
			public const string invln_portaltype = "invln_portaltype";
			public const string invln_contactexternalid = "invln_contactexternalid";
		}
		
		public const string ActionLogicalName = "invln_getcontactrole";
		
		public string invln_contactemail
		{
			get
			{
				if (this.Parameters.Contains("invln_contactemail"))
				{
					return ((string)(this.Parameters["invln_contactemail"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_contactemail"] = value;
			}
		}
		
		public string invln_portaltype
		{
			get
			{
				if (this.Parameters.Contains("invln_portaltype"))
				{
					return ((string)(this.Parameters["invln_portaltype"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_portaltype"] = value;
			}
		}
		
		public string invln_contactexternalid
		{
			get
			{
				if (this.Parameters.Contains("invln_contactexternalid"))
				{
					return ((string)(this.Parameters["invln_contactexternalid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_contactexternalid"] = value;
			}
		}
		
		public invln_getcontactroleRequest()
		{
			this.RequestName = "invln_getcontactrole";
			this.invln_contactemail = default(string);
			this.invln_portaltype = default(string);
			this.invln_contactexternalid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getcontactrole")]
	public partial class invln_getcontactroleResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_portalroles = "invln_portalroles";
		}
		
		public const string ActionLogicalName = "invln_getcontactrole";
		
		public invln_getcontactroleResponse()
		{
		}
		
		public string invln_portalroles
		{
			get
			{
				if (this.Results.Contains("invln_portalroles"))
				{
					return ((string)(this.Results["invln_portalroles"]));
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
