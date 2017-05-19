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
// Generated by IDLImporter from file nsIUpdateTimerManager.idl
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
    /// An interface describing a global application service that allows long
    /// duration (e.g. 1-7 or more days, weeks or months) timers to be registered
    /// and then fired.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0765c92c-6145-4253-9db4-594d8023087e")]
	public interface nsIUpdateTimerManager
	{
		
		/// <summary>
        /// Register an interval with the timer manager. The timer manager
        /// periodically checks to see if the interval has expired and if it has
        /// calls the specified callback. This is persistent across application
        /// restarts and can handle intervals of long durations.
        /// @param   id
        /// An id that identifies the interval, used for persistence
        /// @param   callback
        /// A nsITimerCallback object that is notified when the interval
        /// expires
        /// @param   interval
        /// The length of time, in seconds, of the interval
        ///
        /// Note: to avoid having to instantiate a component to call registerTimer
        /// the component can intead register an update-timer category with comma
        /// separated values as a single string representing the timer as follows.
        ///
        /// _xpcom_categories: [{ category: "update-timer",
        /// value: "contractID," +
        /// "method," +
        /// "id," +
        /// "preference," +
        /// "interval" }],
        /// the values are as follows
        /// contractID : the contract ID for the component.
        /// method     : the method used to instantiate the interface. This should be
        /// either getService or createInstance depending on your
        /// component.
        /// id         : the id that identifies the interval, used for persistence.
        /// preference : the preference to for timer interval. This value can be
        /// optional by specifying an empty string for the value.
        /// interval   : the default interval in seconds for the timer.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RegisterTimer([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase id, [MarshalAs(UnmanagedType.Interface)] nsITimerCallback callback, uint interval);
	}
}