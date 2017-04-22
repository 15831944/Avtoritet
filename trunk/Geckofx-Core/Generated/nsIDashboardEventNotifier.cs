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
// Generated by IDLImporter from file nsIDashboardEventNotifier.idl
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
	[Guid("24fdfcbe-54cb-4997-8392-3c476126ea3b")]
	public interface nsIDashboardEventNotifier
	{
		
		/// <summary>
        ///These methods are called to register a websocket event with the dashboard
        ///
        /// A host is identified by the (aHost, aSerial) pair.
        /// aHost: the host's name: example.com
        /// aSerial: a number that uniquely identifies the websocket
        ///
        /// aEncrypted: if the connection is encrypted
        /// aLength: the length of the message in bytes
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddHost([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aHost, uint aSerial, [MarshalAs(UnmanagedType.U1)] bool aEncrypted);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RemoveHost([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aHost, uint aSerial);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NewMsgSent([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aHost, uint aSerial, uint aLength);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NewMsgReceived([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aHost, uint aSerial, uint aLength);
	}
}
