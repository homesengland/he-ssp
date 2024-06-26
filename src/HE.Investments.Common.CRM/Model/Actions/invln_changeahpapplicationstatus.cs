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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_changeahpapplicationstatus")]
	public partial class invln_changeahpapplicationstatusRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_applicationid = "invln_applicationid";
			public const string invln_organisationid = "invln_organisationid";
			public const string invln_userid = "invln_userid";
			public const string invln_newapplicationstatus = "invln_newapplicationstatus";
			public const string invln_representationsandwarranties = "invln_representationsandwarranties";
			public const string invln_changereason = "invln_changereason";
		}
		
		public const string ActionLogicalName = "invln_changeahpapplicationstatus";
		
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
		
		public int invln_newapplicationstatus
		{
			get
			{
				if (this.Parameters.Contains("invln_newapplicationstatus"))
				{
					return ((int)(this.Parameters["invln_newapplicationstatus"]));
				}
				else
				{
					return default(int);
				}
			}
			set
			{
				this.Parameters["invln_newapplicationstatus"] = value;
			}
		}
		
		public bool invln_representationsandwarranties
		{
			get
			{
				if (this.Parameters.Contains("invln_representationsandwarranties"))
				{
					return ((bool)(this.Parameters["invln_representationsandwarranties"]));
				}
				else
				{
					return default(bool);
				}
			}
			set
			{
				this.Parameters["invln_representationsandwarranties"] = value;
			}
		}
		
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
		
		public invln_changeahpapplicationstatusRequest()
		{
			this.RequestName = "invln_changeahpapplicationstatus";
			this.invln_applicationid = default(string);
			this.invln_organisationid = default(string);
			this.invln_userid = default(string);
			this.invln_newapplicationstatus = default(int);
			this.invln_representationsandwarranties = default(bool);
			this.invln_changereason = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_changeahpapplicationstatus")]
	public partial class invln_changeahpapplicationstatusResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public const string ActionLogicalName = "invln_changeahpapplicationstatus";
		
		public invln_changeahpapplicationstatusResponse()
		{
		}
	}
}
#pragma warning restore CS1591
