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
	
	
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum botComponentcollectionsharingroletype
	{
		
		/// <summary>
		/// Chatbot user has access to the content of the component collection
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Chatbotuser = 1,
		
		/// <summary>
		/// Can author the component collection
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Componentcollectionauthor = 3,
		
		/// <summary>
		/// Has access to the content of the component collection and can add component collection to the copilot
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Componentcollectionuser = 2,
	}
}
#pragma warning restore CS1591
