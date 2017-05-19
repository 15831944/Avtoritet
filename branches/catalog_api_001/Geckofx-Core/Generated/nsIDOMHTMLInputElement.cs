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
// Generated by IDLImporter from file nsIDOMHTMLInputElement.idl
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
    /// The nsIDOMHTMLInputElement interface is the interface to a [X]HTML
    /// input element.
    ///
    /// This interface is trying to follow the DOM Level 2 HTML specification:
    /// http://www.w3.org/TR/DOM-Level-2-HTML/
    ///
    /// with changes from the work-in-progress WHATWG HTML specification:
    /// http://www.whatwg.org/specs/web-apps/current-work/
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("64aeda0b-e9b5-4868-a4f9-e4776e32e733")]
	public interface nsIDOMHTMLInputElement
	{
		
		/// <summary>
        /// The nsIDOMHTMLInputElement interface is the interface to a [X]HTML
        /// input element.
        ///
        /// This interface is trying to follow the DOM Level 2 HTML specification:
        /// http://www.w3.org/TR/DOM-Level-2-HTML/
        ///
        /// with changes from the work-in-progress WHATWG HTML specification:
        /// http://www.whatwg.org/specs/web-apps/current-work/
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAcceptAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAccept);
		
		/// <summary>
        /// The nsIDOMHTMLInputElement interface is the interface to a [X]HTML
        /// input element.
        ///
        /// This interface is trying to follow the DOM Level 2 HTML specification:
        /// http://www.w3.org/TR/DOM-Level-2-HTML/
        ///
        /// with changes from the work-in-progress WHATWG HTML specification:
        /// http://www.whatwg.org/specs/web-apps/current-work/
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAcceptAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAccept);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAltAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAlt);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAltAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAlt);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAutocompleteAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAutocomplete);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAutocompleteAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAutocomplete);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetAutofocusAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAutofocusAttribute([MarshalAs(UnmanagedType.U1)] bool aAutofocus);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetDefaultCheckedAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetDefaultCheckedAttribute([MarshalAs(UnmanagedType.U1)] bool aDefaultChecked);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetCheckedAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetCheckedAttribute([MarshalAs(UnmanagedType.U1)] bool aChecked);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetDisabledAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetDisabledAttribute([MarshalAs(UnmanagedType.U1)] bool aDisabled);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMHTMLFormElement GetFormAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetFormActionAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aFormAction);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFormActionAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aFormAction);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetFormEnctypeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aFormEnctype);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFormEnctypeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aFormEnctype);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetFormMethodAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aFormMethod);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFormMethodAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aFormMethod);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetFormNoValidateAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFormNoValidateAttribute([MarshalAs(UnmanagedType.U1)] bool aFormNoValidate);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetFormTargetAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aFormTarget);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFormTargetAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aFormTarget);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMFileList GetFilesAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetHeightAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetHeightAttribute(uint aHeight);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetIndeterminateAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetIndeterminateAttribute([MarshalAs(UnmanagedType.U1)] bool aIndeterminate);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetInputModeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aInputMode);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetInputModeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aInputMode);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMHTMLElement GetListAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetMaxAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aMax);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetMaxAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aMax);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetMaxLengthAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetMaxLengthAttribute(int aMaxLength);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetMinAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aMin);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetMinAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aMin);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetMultipleAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetMultipleAttribute([MarshalAs(UnmanagedType.U1)] bool aMultiple);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aName);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aName);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetPatternAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aPattern);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPatternAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aPattern);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetPlaceholderAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aPlaceholder);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPlaceholderAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aPlaceholder);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetReadOnlyAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetReadOnlyAttribute([MarshalAs(UnmanagedType.U1)] bool aReadOnly);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetRequiredAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetRequiredAttribute([MarshalAs(UnmanagedType.U1)] bool aRequired);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetStepAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aStep);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetStepAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aStep);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAlignAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAlign);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAlignAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAlign);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetSizeAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSizeAttribute(uint aSize);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetWidthAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetWidthAttribute(uint aWidth);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetSrcAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aSrc);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSrcAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aSrc);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetTypeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aType);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetTypeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aType);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetDefaultValueAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aDefaultValue);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetDefaultValueAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aDefaultValue);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetValueAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aValue);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetValueAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aValue);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetValueAsNumberAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetValueAsNumberAttribute(double aValueAsNumber);
		
		/// <summary>
        /// terrible.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void StepDown(int n, int argc);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void StepUp(int n, int argc);
		
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
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Select();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetSelectionStartAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSelectionStartAttribute(int aSelectionStart);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetSelectionEndAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSelectionEndAttribute(int aSelectionEnd);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSelectionRange(int selectionStart, int selectionEnd, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase direction);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetSelectionDirectionAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aSelectionDirection);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSelectionDirectionAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aSelectionDirection);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetUseMapAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aUseMap);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetUseMapAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aUseMap);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIControllers GetControllersAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetTextLengthAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void MozGetFileNameArray(ref uint aLength, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ref System.IntPtr[] aFileNames);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void MozSetFileNameArray([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] System.IntPtr[] aFileNames, uint aLength);
		
		/// <summary>
        /// This non-standard method prevents to check types manually to know if the
        /// element is a text field.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool MozIsTextField([MarshalAs(UnmanagedType.U1)] bool aExcludePassword);
	}
}