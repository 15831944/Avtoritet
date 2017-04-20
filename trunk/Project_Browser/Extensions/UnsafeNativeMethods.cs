using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace BrowserExtension.Extensions
{
	public class UnsafeNativeMethods
	{
		[Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D"), InterfaceType(2), TypeLibType(4112)]
		[ComImport]
		public interface DWebBrowserEvents2
		{
			[DispId(102)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void StatusTextChange([MarshalAs(UnmanagedType.BStr)] [In] string Text);

			[DispId(108)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void ProgressChange([In] int Progress, [In] int ProgressMax);

			[DispId(105)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void CommandStateChange([In] int Command, [In] bool Enable);

			[DispId(106)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void DownloadBegin();

			[DispId(104)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void DownloadComplete();

			[DispId(113)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void TitleChange([MarshalAs(UnmanagedType.BStr)] [In] string Text);

			[DispId(112)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void PropertyChange([MarshalAs(UnmanagedType.BStr)] [In] string szProperty);

			[DispId(250)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void BeforeNavigate2([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [MarshalAs(UnmanagedType.Struct)] [In] ref object URL, [MarshalAs(UnmanagedType.Struct)] [In] ref object Flags, [MarshalAs(UnmanagedType.Struct)] [In] ref object TargetFrameName, [MarshalAs(UnmanagedType.Struct)] [In] ref object PostData, [MarshalAs(UnmanagedType.Struct)] [In] ref object Headers, [In] [Out] ref bool Cancel);

			[DispId(251)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void NewWindow2([MarshalAs(UnmanagedType.IDispatch)] [In] [Out] ref object ppDisp, [In] [Out] ref bool Cancel);

			[DispId(252)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void NavigateComplete2([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [MarshalAs(UnmanagedType.Struct)] [In] ref object URL);

			[DispId(259)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void DocumentComplete([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [MarshalAs(UnmanagedType.Struct)] [In] ref object URL);

			[DispId(253)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void OnQuit();

			[DispId(254)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void OnVisible([In] bool Visible);

			[DispId(255)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void OnToolBar([In] bool ToolBar);

			[DispId(256)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void OnMenuBar([In] bool MenuBar);

			[DispId(257)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void OnStatusBar([In] bool StatusBar);

			[DispId(258)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void OnFullScreen([In] bool FullScreen);

			[DispId(260)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void OnTheaterMode([In] bool TheaterMode);

			[DispId(262)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void WindowSetResizable([In] bool Resizable);

			[DispId(264)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void WindowSetLeft([In] int Left);

			[DispId(265)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void WindowSetTop([In] int Top);

			[DispId(266)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void WindowSetWidth([In] int Width);

			[DispId(267)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void WindowSetHeight([In] int Height);

			[DispId(263)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void WindowClosing([In] bool IsChildWindow, [In] [Out] ref bool Cancel);

			[DispId(268)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void ClientToHostWindow([In] [Out] ref int CX, [In] [Out] ref int CY);

			[DispId(269)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void SetSecureLockIcon([In] int SecureLockIcon);

			[DispId(270)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void FileDownload([In] [Out] ref bool Cancel);

			[DispId(271)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void NavigateError([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [MarshalAs(UnmanagedType.Struct)] [In] ref object URL, [MarshalAs(UnmanagedType.Struct)] [In] ref object Frame, [MarshalAs(UnmanagedType.Struct)] [In] ref object StatusCode, [In] [Out] ref bool Cancel);

			[DispId(225)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void PrintTemplateInstantiation([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp);

			[DispId(226)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void PrintTemplateTeardown([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp);

			[DispId(227)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void UpdatePageStatus([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [MarshalAs(UnmanagedType.Struct)] [In] ref object nPage, [MarshalAs(UnmanagedType.Struct)] [In] ref object fDone);

			[DispId(272)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void PrivacyImpactedStateChange([In] bool bImpacted);

			[DispId(273)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
			void NewWindow3([MarshalAs(UnmanagedType.IDispatch)] [In] [Out] ref object ppDisp, [In] [Out] ref bool Cancel, [In] uint dwFlags, [MarshalAs(UnmanagedType.BStr)] [In] string bstrUrlContext, [MarshalAs(UnmanagedType.BStr)] [In] string bstrUrl);
		}

		[Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E"), TypeLibType(TypeLibTypeFlags.FHidden | TypeLibTypeFlags.FDual | TypeLibTypeFlags.FOleAutomation), SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IWebBrowser2
		{
			[DispId(200)]
			object Application
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			[DispId(201)]
			object Parent
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			[DispId(202)]
			object Container
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			[DispId(203)]
			object Document
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			[DispId(204)]
			bool TopLevelContainer
			{
				get;
			}

			[DispId(205)]
			string Type
			{
				get;
			}

			[DispId(206)]
			int Left
			{
				get;
				set;
			}

			[DispId(207)]
			int Top
			{
				get;
				set;
			}

			[DispId(208)]
			int Width
			{
				get;
				set;
			}

			[DispId(209)]
			int Height
			{
				get;
				set;
			}

			[DispId(210)]
			string LocationName
			{
				get;
			}

			[DispId(211)]
			string LocationURL
			{
				get;
			}

			[DispId(212)]
			bool Busy
			{
				get;
			}

			[DispId(0)]
			string Name
			{
				get;
			}

			[DispId(-515)]
			int HWND
			{
				get;
			}

			[DispId(400)]
			string FullName
			{
				get;
			}

			[DispId(401)]
			string Path
			{
				get;
			}

			[DispId(402)]
			bool Visible
			{
				get;
				set;
			}

			[DispId(403)]
			bool StatusBar
			{
				get;
				set;
			}

			[DispId(404)]
			string StatusText
			{
				get;
				set;
			}

			[DispId(405)]
			int ToolBar
			{
				get;
				set;
			}

			[DispId(406)]
			bool MenuBar
			{
				get;
				set;
			}

			[DispId(407)]
			bool FullScreen
			{
				get;
				set;
			}

			[DispId(-525)]
			WebBrowserReadyState ReadyState
			{
				get;
			}

			[DispId(550)]
			bool Offline
			{
				get;
				set;
			}

			[DispId(551)]
			bool Silent
			{
				get;
				set;
			}

			[DispId(552)]
			bool RegisterAsBrowser
			{
				get;
				set;
			}

			[DispId(553)]
			bool RegisterAsDropTarget
			{
				get;
				set;
			}

			[DispId(554)]
			bool TheaterMode
			{
				get;
				set;
			}

			[DispId(555)]
			bool AddressBar
			{
				get;
				set;
			}

			[DispId(556)]
			bool Resizable
			{
				get;
				set;
			}

			[DispId(100)]
			void GoBack();

			[DispId(101)]
			void GoForward();

			[DispId(102)]
			void GoHome();

			[DispId(103)]
			void GoSearch();

			[DispId(104)]
			void Navigate([In] string Url, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);

			[DispId(-550)]
			void Refresh();

			[DispId(105)]
			void Refresh2([In] ref object level);

			[DispId(106)]
			void Stop();

			[DispId(300)]
			void Quit();

			[DispId(301)]
			void ClientToWindow(out int pcx, out int pcy);

			[DispId(302)]
			void PutProperty([In] string property, [In] object vtValue);

			[DispId(303)]
			object GetProperty([In] string property);

			[DispId(500)]
			void Navigate2([In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);

			[DispId(501)]
			NativeMethods.OLECMDF QueryStatusWB([In] NativeMethods.OLECMDID cmdID);

			[DispId(502)]
			void ExecWB([In] NativeMethods.OLECMDID cmdID, [In] NativeMethods.OLECMDEXECOPT cmdexecopt, ref object pvaIn, IntPtr pvaOut);

			[DispId(503)]
			void ShowBrowserBar([In] ref object pvaClsid, [In] ref object pvarShow, [In] ref object pvarSize);
		}

		private UnsafeNativeMethods()
		{
		}
	}
}
