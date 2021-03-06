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
// Generated by IDLImporter from file nsIDOMHTMLObjectElement.idl
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
    /// The nsIDOMHTMLObjectElement interface is the interface to a [X]HTML
    /// object element.
    ///
    /// This interface is trying to follow the DOM Level 2 HTML specification:
    /// http://www.w3.org/TR/DOM-Level-2-HTML/
    ///
    /// with changes from the work-in-progress WHATWG HTML specification:
    /// http://www.whatwg.org/specs/web-apps/current-work/
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("baf443d2-da5d-40c9-be3c-c65a69a25250")]
	public interface nsIDOMHTMLObjectElement
	{
		
		/// <summary>
        /// The nsIDOMHTMLObjectElement interface is the interface to a [X]HTML
        /// object element.
        ///
        /// This interface is trying to follow the DOM Level 2 HTML specification:
        /// http://www.w3.org/TR/DOM-Level-2-HTML/
        ///
        /// with changes from the work-in-progress WHATWG HTML specification:
        /// http://www.whatwg.org/specs/web-apps/current-work/
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMHTMLFormElement GetFormAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCodeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aCode);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetCodeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aCode);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAlignAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAlign);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAlignAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAlign);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetArchiveAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aArchive);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetArchiveAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aArchive);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetBorderAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aBorder);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetBorderAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aBorder);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCodeBaseAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aCodeBase);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetCodeBaseAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aCodeBase);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCodeTypeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aCodeType);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetCodeTypeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aCodeType);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetDataAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aData);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetDataAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aData);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetDeclareAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetDeclareAttribute([MarshalAs(UnmanagedType.U1)] bool aDeclare);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetHeightAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aHeight);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetHeightAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aHeight);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetHspaceAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetHspaceAttribute(int aHspace);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aName);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aName);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetStandbyAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aStandby);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetStandbyAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aStandby);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetTypeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aType);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetTypeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aType);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetUseMapAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aUseMap);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetUseMapAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aUseMap);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetVspaceAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetVspaceAttribute(int aVspace);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetWidthAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aWidth);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetWidthAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aWidth);
		
		/// <summary>
        /// Introduced in DOM Level 2:
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDocument GetContentDocumentAttribute();
		
		/// <summary>
        /// http://www.whatwg.org/specs/web-apps/current-work/#the-constraint-validation-api
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetWillValidateAttribute();
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMValidityState GetValidityAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetValidationMessageAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aValidationMessage);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool CheckValidity();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetCustomValidity([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase error);
	}
}
