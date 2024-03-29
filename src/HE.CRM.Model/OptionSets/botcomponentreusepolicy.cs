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
	/// Type of Reuse Policy associated with Power Virtual Agents chatbot subcomponents.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum botcomponentreusepolicy
	{
		
		/// <summary>
		/// Not Reusable. By default, a chatbot subcomponent is not reusable and Reuse Policy is None
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		None = 0,
		
		/// <summary>
		/// Is required by one or more Public chatbot subcomponent, but is not directly invokable or visible
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Private = 1,
		
		/// <summary>
		/// Visible shared / reusable chatbot subcomponent for use in all bots in the environment
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Public = 2,
	}
}
#pragma warning restore CS1591
