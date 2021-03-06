// --------------------------------------------------------------------------------------------
// Version: MPL 1.1/GPL 2.0/LGPL 2.1
// 
// The contents of this file are subject to the Mozilla Public License Version
// 1.1 (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
// for the specific language governing rights and limitations under the
// License.
// 
// <remarks>
// Generated by IDLImporter from file nsISAXMutableAttributes.idl
// 
// You should use these interfaces when you access the COM objects defined in the mentioned
// IDL/IDH file.
// </remarks>
// --------------------------------------------------------------------------------------------
namespace Gecko
{
	using System;
	using System.Runtime.InteropServices;
	using System.Runtime.InteropServices.ComTypes;
	using System.Runtime.CompilerServices;
	
	
	/// <summary>
    /// This interface extends the nsISAXAttributes interface with
    /// manipulators so that the list can be modified or reused.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8b1de83d-cebb-49fa-8245-c0fe319eb7b6")]
	public interface nsISAXMutableAttributes : nsISAXAttributes
	{
		
		/// <summary>
        /// Look up the index of an attribute by Namespace name.
        /// @param uri The Namespace URI, or the empty string
        /// if the name has no Namespace URI.
        /// @param localName The attribute's local name.
        /// @return The index of the attribute, or -1
        /// if it does not appear in the list.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetIndexFromName([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase uri, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase localName);
		
		/// <summary>
        /// Look up the index of an attribute by XML qualified name.
        /// @param qName The qualified name.
        /// @return The index of the attribute, or -1
        /// if it does not appear in the list.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetIndexFromQName([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase qName);
		
		/// <summary>
        /// Return the number of attributes in the list. Once you know the
        /// number of attributes, you can iterate through the list.
        ///
        /// @return The number of attributes in the list.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetLengthAttribute();
		
		/// <summary>
        /// Look up an attribute's local name by index.
        /// @param index The attribute index (zero-based).
        /// @return The local name, or null if the index is out of range.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetLocalName(uint index, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// Look up an attribute's XML qualified name by index.
        /// @param index The attribute index (zero-based).
        /// @return The XML qualified name, or the empty string if none is
        /// available, or null if the index is out of range.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetQName(uint index, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// Look up an attribute's type by index. The attribute type is one
        /// of the strings "CDATA", "ID", "IDREF", "IDREFS", "NMTOKEN",
        /// "NMTOKENS", "ENTITY", "ENTITIES", or "NOTATION" (always in upper
        /// case). If the parser has not read a declaration for the
        /// attribute, or if the parser does not report attribute types, then
        /// it must return the value "CDATA" as stated in the XML 1.0
        /// Recommendation (clause 3.3.3, "Attribute-Value
        /// Normalization"). For an enumerated attribute that is not a
        /// notation, the parser will report the type as "NMTOKEN".
        ///
        /// @param index The attribute index (zero-based).
        /// @return The attribute's type as a string, or null if the index is
        /// out of range.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetType(uint index, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// Look up an attribute's type by Namespace name.
        /// @param uri The Namespace URI, or the empty string
        /// if the name has no Namespace URI.
        /// @param localName The attribute's local name.
        /// @return The attribute type as a string, or null if the attribute
        /// is not in the list.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetTypeFromName([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase uri, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase localName, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// Look up an attribute's type by XML qualified name.
        /// @param qName The qualified name.
        /// @return The attribute type as a string, or null if the attribute
        /// is not in the list.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetTypeFromQName([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase qName, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// Look up an attribute's Namespace URI by index.
        /// @param index The attribute index (zero-based).
        /// @return The Namespace URI, or the empty string if none is available,
        /// or null if the index is out of range.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetURI(uint index, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// Look up an attribute's value by index.  If the attribute value is
        /// a list of tokens (IDREFS, ENTITIES, or NMTOKENS), the tokens will
        /// be concatenated into a single string with each token separated by
        /// a single space.
        ///
        /// @param index The attribute index (zero-based).
        /// @return The attribute's value as a string, or null if the index is
        /// out of range.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetValue(uint index, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// Look up an attribute's value by Namespace name.  If the attribute
        /// value is a list of tokens (IDREFS, ENTITIES, or NMTOKENS), the
        /// tokens will be concatenated into a single string with each token
        /// separated by a single space.
        ///
        /// @param uri The Namespace URI, or the empty string
        /// if the name has no Namespace URI.
        /// @param localName The attribute's local name.
        /// @return The attribute's value as a string, or null if the attribute is
        /// not in the list.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetValueFromName([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase uri, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase localName, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// Look up an attribute's value by XML qualified (prefixed) name.
        /// If the attribute value is a list of tokens (IDREFS, ENTITIES, or
        /// NMTOKENS), the tokens will be concatenated into a single string
        /// with each token separated by a single space.
        ///
        /// @param qName The qualified (prefixed) name.
        /// @return The attribute's value as a string, or null if the attribute is
        /// not in the list.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetValueFromQName([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase qName, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// Add an attribute to the end of the list.
        ///
        /// For the sake of speed, this method does no checking
        /// to see if the attribute is already in the list: that is
        /// the responsibility of the application.
        ///
        /// @param uri The Namespace URI, or the empty string if
        /// none is available or Namespace processing is not
        /// being performed.
        /// @param localName The local name, or the empty string if
        /// Namespace processing is not being performed.
        /// @param qName The qualified (prefixed) name, or the empty string
        /// if qualified names are not available.
        /// @param type The attribute type as a string.
        /// @param value The attribute value.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase uri, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase localName, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase qName, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase type, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase value);
		
		/// <summary>
        /// Clear the attribute list for reuse.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Clear();
		
		/// <summary>
        /// Remove an attribute from the list.
        ///
        /// @param index The index of the attribute (zero-based).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RemoveAttribute(uint index);
		
		/// <summary>
        /// Set the attributes list. This method will clear any attributes in
        /// the list before adding the attributes from the argument.
        ///
        /// @param attributes The attributes object to replace populate the
        /// list with.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAttributes([MarshalAs(UnmanagedType.Interface)] nsISAXAttributes attributes);
		
		/// <summary>
        /// Set an attribute in the list.
        ///
        /// For the sake of speed, this method does no checking for name
        /// conflicts or well-formedness: such checks are the responsibility
        /// of the application.
        ///
        /// @param index The index of the attribute (zero-based).
        /// @param uri The Namespace URI, or the empty string if
        /// none is available or Namespace processing is not
        /// being performed.
        /// @param localName The local name, or the empty string if
        /// Namespace processing is not being performed.
        /// @param qName The qualified name, or the empty string
        /// if qualified names are not available.
        /// @param type The attribute type as a string.
        /// @param value The attribute value.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAttribute(uint index, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase uri, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase localName, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase qName, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase type, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase value);
		
		/// <summary>
        /// Set the local name of a specific attribute.
        ///
        /// @param index The index of the attribute (zero-based).
        /// @param localName The attribute's local name, or the empty
        /// string for none.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetLocalName(uint index, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase localName);
		
		/// <summary>
        /// Set the qualified name of a specific attribute.
        ///
        /// @param index The index of the attribute (zero-based).
        /// @param qName The attribute's qualified name, or the empty
        /// string for none.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetQName(uint index, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase qName);
		
		/// <summary>
        /// Set the type of a specific attribute.
        ///
        /// @param index The index of the attribute (zero-based).
        /// @param type The attribute's type.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetType(uint index, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase type);
		
		/// <summary>
        /// Set the Namespace URI of a specific attribute.
        ///
        /// @param index The index of the attribute (zero-based).
        /// @param uri The attribute's Namespace URI, or the empty
        /// string for none.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetURI(uint index, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase uri);
		
		/// <summary>
        /// Set the value of a specific attribute.
        ///
        /// @param index The index of the attribute (zero-based).
        /// @param value The attribute's value.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetValue(uint index, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase value);
	}
}
