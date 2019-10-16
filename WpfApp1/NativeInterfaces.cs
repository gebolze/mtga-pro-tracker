using System;
using System.Runtime.InteropServices;

namespace MTGApro
{
    internal static class NativeInterfaces
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr GetForegroundWindow();
    }
}