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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getmultiplelocalauthoritiesformodule")]
	public partial class invln_getmultiplelocalauthoritiesformoduleRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_pagingrequest = "invln_pagingrequest";
			public const string invln_searchphrase = "invln_searchphrase";
			public const string invln_isloan = "invln_isloan";
			public const string invln_isahp = "invln_isahp";
			public const string invln_isloanfd = "invln_isloanfd";
		}
		
		public const string ActionLogicalName = "invln_getmultiplelocalauthoritiesformodule";
		
		public string invln_pagingrequest
		{
			get
			{
				if (this.Parameters.Contains("invln_pagingrequest"))
				{
					return ((string)(this.Parameters["invln_pagingrequest"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_pagingrequest"] = value;
			}
		}
		
		public string invln_searchphrase
		{
			get
			{
				if (this.Parameters.Contains("invln_searchphrase"))
				{
					return ((string)(this.Parameters["invln_searchphrase"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_searchphrase"] = value;
			}
		}
		
		public string invln_isloan
		{
			get
			{
				if (this.Parameters.Contains("invln_isloan"))
				{
					return ((string)(this.Parameters["invln_isloan"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_isloan"] = value;
			}
		}
		
		public string invln_isahp
		{
			get
			{
				if (this.Parameters.Contains("invln_isahp"))
				{
					return ((string)(this.Parameters["invln_isahp"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_isahp"] = value;
			}
		}
		
		public string invln_isloanfd
		{
			get
			{
				if (this.Parameters.Contains("invln_isloanfd"))
				{
					return ((string)(this.Parameters["invln_isloanfd"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_isloanfd"] = value;
			}
		}
		
		public invln_getmultiplelocalauthoritiesformoduleRequest()
		{
			this.RequestName = "invln_getmultiplelocalauthoritiesformodule";
			this.invln_pagingrequest = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getmultiplelocalauthoritiesformodule")]
	public partial class invln_getmultiplelocalauthoritiesformoduleResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_localauthorities = "invln_localauthorities";
		}
		
		public const string ActionLogicalName = "invln_getmultiplelocalauthoritiesformodule";
		
		public invln_getmultiplelocalauthoritiesformoduleResponse()
		{
		}
		
		public string invln_localauthorities
		{
			get
			{
				if (this.Results.Contains("invln_localauthorities"))
				{
					return ((string)(this.Results["invln_localauthorities"]));
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
