namespace NewLauncher.Helper
{
    using NewLauncher.Interop;
    using System;
    using System.Runtime.InteropServices;

    public static class InteropHelper
    {
        [DllImport("kernel32.dll", SetLastError=true)]
        public static extern void GetSystemTime(ref SystemTime sysTime);
        [DllImport("kernel32.dll", SetLastError=true)]
        public static extern bool SetSystemTime(ref SystemTime sysTime);
    }
}

