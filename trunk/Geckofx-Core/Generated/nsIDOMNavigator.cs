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
// Generated by IDLImporter from file nsIDOMNavigator.idl
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
    ///This Source Code Form is subject to the terms of the Mozilla Public
    /// License, v. 2.0. If a copy of the MPL was not distributed with this
    /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("f1101fbb-d119-4cb8-845b-6bbae8a151c7")]
	public interface nsIDOMNavigator
	{
		
		/// <summary>
        ///This Source Code Form is subject to the terms of the Mozilla Public
        /// License, v. 2.0. If a copy of the MPL was not distributed with this
        /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAppCodeNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAppCodeName);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAppNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAppName);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAppVersionAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAppVersion);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetLanguageAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aLanguage);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetPlatformAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aPlatform);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetOscpuAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aOscpu);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetVendorAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aVendor);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetVendorSubAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aVendorSub);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetProductAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aProduct);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetProductSubAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aProductSub);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetUserAgentAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aUserAgent);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetBuildIDAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aBuildID);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetDoNotTrackAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aDoNotTrack);
	}
}