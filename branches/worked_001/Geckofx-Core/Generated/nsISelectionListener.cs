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
// Generated by IDLImporter from file nsISelectionListener.idl
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
	[Guid("280cd784-23c2-468d-8624-354e0b3804bd")]
	public interface nsISelectionListener
	{
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifySelectionChanged([MarshalAs(UnmanagedType.Interface)] nsIDOMDocument doc, [MarshalAs(UnmanagedType.Interface)] nsISelection sel, short reason);
	}
	
	/// <summary>nsISelectionListenerConsts </summary>
	public class nsISelectionListenerConsts
	{
		
		// <summary>
        //This Source Code Form is subject to the terms of the Mozilla Public
        // License, v. 2.0. If a copy of the MPL was not distributed with this
        // file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		public const int NO_REASON = 0;
		
		// 
		public const int DRAG_REASON = 1;
		
		// 
		public const int MOUSEDOWN_REASON = 2;
		
		// <summary>
        //bitflags </summary>
		public const int MOUSEUP_REASON = 4;
		
		// <summary>
        //bitflags </summary>
		public const int KEYPRESS_REASON = 8;
		
		// <summary>
        //bitflags </summary>
		public const int SELECTALL_REASON = 16;
		
		// 
		public const int COLLAPSETOSTART_REASON = 32;
		
		// 
		public const int COLLAPSETOEND_REASON = 64;
	}
}
