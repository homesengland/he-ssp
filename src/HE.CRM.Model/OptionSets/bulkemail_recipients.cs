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
	
	
	/// <summary>
	/// Select the records to send the direct email to
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum bulkemail_recipients
	{
		
		/// <summary>
		/// Send direct email to all the records on all the pages in the current view.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Allrecordsonallpages = 3,
		
		/// <summary>
		/// Send direct email to all the records on this page.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Allrecordsoncurrentpage = 2,
		
		/// <summary>
		/// Send direct email only to the records you selected on this page.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Selectedrecordsoncurrentpage = 1,
	}
}
#pragma warning restore CS1591
