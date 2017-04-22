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
// Generated by IDLImporter from file nsIMemoryReporter.idl
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
    /// Memory reporters measure Firefox's memory usage.  They are primarily used to
    /// generate the about:memory page.  You should read
    /// https://wiki.mozilla.org/Memory_Reporting before writing a memory
    /// reporter.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3a61be3b-b93b-461a-a4f8-388214f558b1")]
	public interface nsIMemoryReporterCallback
	{
		
		/// <summary>
        /// The arguments to the callback are as follows.
        ///
        ///
        /// |process|  The name of the process containing this reporter.  Each
        /// reporter initially has "" in this field, indicating that it applies to the
        /// current process.  (This is true even for reporters in a child process.)
        /// When a reporter from a child process is copied into the main process, the
        /// copy has its 'process' field set appropriately.
        ///
        ///
        /// |path|  The path that this memory usage should be reported under.  Paths
        /// are '/'-delimited, eg. "a/b/c".
        ///
        /// Each reporter can be viewed as representing a leaf node in a tree.
        /// Internal nodes of the tree don't have reporters.  So, for example, the
        /// reporters "explicit/a/b", "explicit/a/c", "explicit/d/e", and
        /// "explicit/d/f" define this tree:
        ///
        /// explicit
        /// |--a
        /// |  |--b [*]
        /// |  \--c [*]
        /// \--d
        /// |--e [*]
        /// \--f [*]
        ///
        /// Nodes marked with a [*] have a reporter.  Notice that the internal
        /// nodes are implicitly defined by the paths.
        ///
        /// Nodes within a tree should not overlap measurements, otherwise the
        /// parent node measurements will be double-counted.  So in the example
        /// above, |b| should not count any allocations counted by |c|, and vice
        /// versa.
        ///
        /// All nodes within each tree must have the same units.
        ///
        /// If you want to include a '/' not as a path separator, e.g. because the
        /// path contains a URL, you need to convert each '/' in the URL to a '\'.
        /// Consumers of the path will undo this change.  Any other '\' character
        /// in a path will also be changed.  This is clumsy but hasn't caused any
        /// problems so far.
        ///
        /// The paths of all reporters form a set of trees.  Trees can be
        /// "degenerate", i.e. contain a single entry with no '/'.
        ///
        ///
        /// |kind|  There are three kinds of memory reporters.
        ///
        /// - HEAP: reporters measuring memory allocated by the heap allocator,
        /// e.g. by calling malloc, calloc, realloc, memalign, operator new, or
        /// operator new[].  Reporters in this category must have units
        /// UNITS_BYTES.
        ///
        /// - NONHEAP: reporters measuring memory which the program explicitly
        /// allocated, but does not live on the heap.  Such memory is commonly
        /// allocated by calling one of the OS's memory-mapping functions (e.g.
        /// mmap, VirtualAlloc, or vm_allocate).  Reporters in this category
        /// must have units UNITS_BYTES.
        ///
        /// - OTHER: reporters which don't fit into either of these categories.
        /// They can have any units.
        ///
        /// The kind only matters for reporters in the "explicit" tree;
        /// aboutMemory.js uses it to calculate "heap-unclassified".
        ///
        ///
        /// |units|  The units on the reporter's amount.  One of the following.
        ///
        /// - BYTES: The amount contains a number of bytes.
        ///
        /// - COUNT: The amount is an instantaneous count of things currently in
        /// existence.  For instance, the number of tabs currently open would have
        /// units COUNT.
        ///
        /// - COUNT_CUMULATIVE: The amount contains the number of times some event
        /// has occurred since the application started up.  For instance, the
        /// number of times the user has opened a new tab would have units
        /// COUNT_CUMULATIVE.
        ///
        /// The amount returned by a reporter with units COUNT_CUMULATIVE must
        /// never decrease over the lifetime of the application.
        ///
        /// - PERCENTAGE: The amount contains a fraction that should be expressed as
        /// a percentage.  NOTE!  The |amount| field should be given a value 100x
        /// the actual percentage;  this number will be divided by 100 when shown.
        /// This allows a fractional percentage to be shown even though |amount| is
        /// an integer.  E.g. if the actual percentage is 12.34%, |amount| should
        /// be 1234.
        ///
        /// Values greater than 100% are allowed.
        ///
        ///
        /// |amount|  The numeric value reported by this memory reporter.  Accesses
        /// can fail if something goes wrong when getting the amount.
        ///
        ///
        /// |description|  A human-readable description of this memory usage report.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Callback([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase process, [MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase path, int kind, int units, long amount, [MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase description, [MarshalAs(UnmanagedType.Interface)] nsISupports data);
	}
	
	/// <summary>
    /// An nsIMemoryReporter reports one or more memory measurements via a
    /// callback function which is called once for each measurement.
    ///
    /// An nsIMemoryReporter that reports a single measurement is sometimes called a
    /// "uni-reporter".  One that reports multiple measurements is sometimes called
    /// a "multi-reporter".
    ///
    /// aboutMemory.js is the most important consumer of memory reports.  It
    /// places the following constraints on reports.
    ///
    /// - All reports within a single sub-tree must have the same units.
    ///
    /// - There may be an "explicit" tree.  If present, it represents
    /// non-overlapping regions of memory that have been explicitly allocated with
    /// an OS-level allocation (e.g. mmap/VirtualAlloc/vm_allocate) or a
    /// heap-level allocation (e.g. malloc/calloc/operator new).  Reporters in
    /// this tree must have kind HEAP or NONHEAP, units BYTES.
    ///
    /// It is preferred, but not required, that report descriptions use complete
    /// sentences (i.e. start with a capital letter and end with a period, or
    /// similar).
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0884cd0f-5829-4381-979b-0f53904030ed")]
	public interface nsIMemoryReporter
	{
		
		/// <summary>
        /// Run the reporter.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CollectReports([MarshalAs(UnmanagedType.Interface)] nsIMemoryReporterCallback callback, [MarshalAs(UnmanagedType.Interface)] nsISupports data);
	}
	
	/// <summary>nsIMemoryReporterConsts </summary>
	public class nsIMemoryReporterConsts
	{
		
		// <summary>
        // Kinds.  See the |kind| comment in nsIMemoryReporterCallback.
        // </summary>
		public const long KIND_NONHEAP = 0;
		
		// 
		public const long KIND_HEAP = 1;
		
		// 
		public const long KIND_OTHER = 2;
		
		// <summary>
        // Units.  See the |units| comment in nsIMemoryReporterCallback.
        // </summary>
		public const long UNITS_BYTES = 0;
		
		// 
		public const long UNITS_COUNT = 1;
		
		// 
		public const long UNITS_COUNT_CUMULATIVE = 2;
		
		// 
		public const long UNITS_PERCENTAGE = 3;
	}
	
	/// <summary>nsIFinishReportingCallback </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("548b3909-c04d-4ca6-8466-b8bee3837457")]
	public interface nsIFinishReportingCallback
	{
		
		/// <summary>Member Callback </summary>
		/// <param name='data'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Callback([MarshalAs(UnmanagedType.Interface)] nsISupports data);
	}
	
	/// <summary>nsIMemoryReporterManager </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("e4e4ca56-13e0-46f1-b3c5-62d2c09fc98e")]
	public interface nsIMemoryReporterManager
	{
		
		/// <summary>
        /// Initialize.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Init();
		
		/// <summary>
        /// Register the given nsIMemoryReporter.  The Manager service will hold a
        /// strong reference to the given reporter, and will be responsible for freeing
        /// the reporter at shutdown.  You may manually unregister the reporter with
        /// unregisterStrongReporter() at any point.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RegisterStrongReporter([MarshalAs(UnmanagedType.Interface)] nsIMemoryReporter reporter);
		
		/// <summary>
        /// Like registerReporter, but the Manager service will hold a weak reference
        /// via a raw pointer to the given reporter.  The reporter should be
        /// unregistered before shutdown.
        /// You cannot register JavaScript components with this function!  Always
        /// register your JavaScript components with registerStrongReporter().
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RegisterWeakReporter([MarshalAs(UnmanagedType.Interface)] nsIMemoryReporter reporter);
		
		/// <summary>
        /// Unregister the given memory reporter, which must have been registered with
        /// registerStrongReporter().  You normally don't need to unregister your
        /// strong reporters, as nsIMemoryReporterManager will take care of that at
        /// shutdown.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnregisterStrongReporter([MarshalAs(UnmanagedType.Interface)] nsIMemoryReporter reporter);
		
		/// <summary>
        /// Unregister the given memory reporter, which must have been registered with
        /// registerWeakReporter().
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnregisterWeakReporter([MarshalAs(UnmanagedType.Interface)] nsIMemoryReporter reporter);
		
		/// <summary>
        /// These functions should only be used for testing purposes.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void BlockRegistrationAndHideExistingReporters();
		
		/// <summary>Member UnblockRegistrationAndRestoreOriginalReporters </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnblockRegistrationAndRestoreOriginalReporters();
		
		/// <summary>Member RegisterStrongReporterEvenIfBlocked </summary>
		/// <param name='aReporter'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RegisterStrongReporterEvenIfBlocked([MarshalAs(UnmanagedType.Interface)] nsIMemoryReporter aReporter);
		
		/// <summary>
        /// Get memory reports for the current process and all child processes.
        /// |handleReport| is called for each report, and |finishReporting| is called
        /// once all reports have been handled.
        ///
        /// |finishReporting| is called even if, for example, some child processes
        /// fail to report back.  However, calls to this method will silently and
        /// immediately abort -- and |finishReporting| will not be called -- if a
        /// previous getReports() call is still in flight, i.e. if it has not yet
        /// finished invoking |finishReporting|.  The silent abort is because the
        /// in-flight request will finish soon, and the caller would very likely just
        /// catch and ignore any error anyway.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetReports([MarshalAs(UnmanagedType.Interface)] nsIMemoryReporterCallback handleReport, [MarshalAs(UnmanagedType.Interface)] nsISupports handleReportData, [MarshalAs(UnmanagedType.Interface)] nsIFinishReportingCallback finishReporting, [MarshalAs(UnmanagedType.Interface)] nsISupports finishReportingData);
		
		/// <summary>
        /// Get memory reports in the current process only.  |handleReport| is called
        /// for each report.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetReportsForThisProcess([MarshalAs(UnmanagedType.Interface)] nsIMemoryReporterCallback handleReport, [MarshalAs(UnmanagedType.Interface)] nsISupports handleReportData);
		
		/// <summary>
        /// The memory reporter manager, for the most part, treats reporters
        /// registered with it as a black box.  However, there are some
        /// "distinguished" amounts (as could be reported by a memory reporter) that
        /// the manager provides as attributes, because they are sufficiently
        /// interesting that we want external code (e.g. telemetry) to be able to rely
        /// on them.
        ///
        /// Note that these are not reporters and so getReports() and
        /// getReportsForThisProcess() do not look at them.  However, distinguished
        /// amounts can be embedded in a reporter.
        ///
        /// Access to these attributes can fail.  In particular, some of them are not
        /// available on all platforms.
        ///
        /// If you add a new distinguished amount, please update
        /// toolkit/components/aboutmemory/tests/test_memoryReporters.xul.
        ///
        /// |explicit| (UNITS_BYTES)  The total size of explicit memory allocations,
        /// both at the OS-level (eg. via mmap, VirtualAlloc) and at the heap level
        /// (eg. via malloc, calloc, operator new).  It covers all heap allocations,
        /// but will miss any OS-level ones not covered by memory reporters.
        ///
        /// |vsize| (UNITS_BYTES)  The virtual size, i.e. the amount of address space
        /// taken up.
        ///
        /// |vsizeMaxContiguous| (UNITS_BYTES)  The size of the largest contiguous
        /// block of virtual memory.
        ///
        /// |resident| (UNITS_BYTES)  The resident size (a.k.a. RSS or physical memory
        /// used).
        ///
        /// |residentFast| (UNITS_BYTES)  This is like |resident|, but on Mac OS
        /// |resident| can purge pages, which is slow.  It also affects the result of
        /// |residentFast|, and so |resident| and |residentFast| should not be used
        /// together.
        ///
        /// |heapAllocated| (UNITS_BYTES)  Memory mapped by the heap allocator.
        ///
        /// |heapOverheadRatio| (UNITS_PERCENTAGE)  In the heap allocator, this is the
        /// ratio of committed, unused bytes to allocated bytes.  Like all
        /// UNITS_PERCENTAGE measurements, its amount is multiplied by 100x so it can
        /// be represented by an int64_t.
        ///
        /// |JSMainRuntimeGCHeap| (UNITS_BYTES)  Size of the main JS runtime's GC
        /// heap.
        ///
        /// |JSMainRuntimeTemporaryPeak| (UNITS_BYTES)  Peak size of the transient
        /// storage in the main JSRuntime.
        ///
        /// |JSMainRuntimeCompartments{System,User}| (UNITS_COUNT)  The number of
        /// {system,user} compartments in the main JS runtime.
        ///
        /// |imagesContentUsedUncompressed| (UNITS_BYTES)  Memory used for decoded
        /// images in content.
        ///
        /// |storageSQLite| (UNITS_BYTES)  Memory used by SQLite.
        ///
        /// |lowMemoryEvents{Virtual,Physical}| (UNITS_COUNT_CUMULATIVE)  The number
        /// of low-{virtual,physical}-memory events that have occurred since the
        /// process started.
        ///
        /// |ghostWindows| (UNITS_COUNT)  The number of ghost windows.
        ///
        /// |pageFaultsHard| (UNITS_COUNT_CUMULATIVE)  The number of hard (a.k.a.
        /// major) page faults that have occurred since the process started.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetExplicitAttribute();
		
		/// <summary>Member GetVsizeAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetVsizeAttribute();
		
		/// <summary>Member GetVsizeMaxContiguousAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetVsizeMaxContiguousAttribute();
		
		/// <summary>Member GetResidentAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetResidentAttribute();
		
		/// <summary>Member GetResidentFastAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetResidentFastAttribute();
		
		/// <summary>Member GetHeapAllocatedAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetHeapAllocatedAttribute();
		
		/// <summary>Member GetHeapOverheadRatioAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetHeapOverheadRatioAttribute();
		
		/// <summary>Member GetJSMainRuntimeGCHeapAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetJSMainRuntimeGCHeapAttribute();
		
		/// <summary>Member GetJSMainRuntimeTemporaryPeakAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetJSMainRuntimeTemporaryPeakAttribute();
		
		/// <summary>Member GetJSMainRuntimeCompartmentsSystemAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetJSMainRuntimeCompartmentsSystemAttribute();
		
		/// <summary>Member GetJSMainRuntimeCompartmentsUserAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetJSMainRuntimeCompartmentsUserAttribute();
		
		/// <summary>Member GetImagesContentUsedUncompressedAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetImagesContentUsedUncompressedAttribute();
		
		/// <summary>Member GetStorageSQLiteAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetStorageSQLiteAttribute();
		
		/// <summary>Member GetLowMemoryEventsVirtualAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetLowMemoryEventsVirtualAttribute();
		
		/// <summary>Member GetLowMemoryEventsPhysicalAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetLowMemoryEventsPhysicalAttribute();
		
		/// <summary>Member GetGhostWindowsAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetGhostWindowsAttribute();
		
		/// <summary>Member GetPageFaultsHardAttribute </summary>
		/// <returns>A System.Int64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetPageFaultsHardAttribute();
		
		/// <summary>
        /// This attribute indicates if moz_malloc_usable_size() works.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetHasMozMallocUsableSizeAttribute();
		
		/// <summary>
        /// Run a series of GC/CC's in an attempt to minimize the application's memory
        /// usage.  When we're finished, we invoke the given runnable if it's not
        /// null.  Returns a reference to the runnable used for carrying out the task.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsICancelableRunnable MinimizeMemoryUsage([MarshalAs(UnmanagedType.Interface)] nsIRunnable callback);
		
		/// <summary>
        /// Measure the memory that is known to be owned by this tab, split up into
        /// several broad categories.  Note that this will be an underestimate of the
        /// true number, due to imperfect memory reporter coverage (corresponding to
        /// about:memory's "heap-unclassified"), and due to some memory shared between
        /// tabs not being counted.
        ///
        /// The time taken for the measurement (split into JS and non-JS parts) is
        /// also returned.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SizeOfTab([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, ref long jsObjectsSize, ref long jsStringsSize, ref long jsOtherSize, ref long domSize, ref long styleSize, ref long otherSize, ref long totalSize, ref double jsMilliseconds, ref double nonJSMilliseconds);
	}
}
