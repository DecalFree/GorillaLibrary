using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GorillaLibrary.Utilities;

// this class used to have a replacement for Environment.GetFolderPath() but it's Linux support was janky so it has been dropped for now
// this may appear again in the future.
// adapted from https://github.com/sirkingbinx/cardboard/blob/master/src/Classes/UserSystem.cs#L20

/// <summary>
/// Utilities for system tasks.
/// </summary>
public static class SystemUtility
{
    /// <summary>
    /// The operating system the environment is running on.
    /// Note that this also includes detection for Wine, which will be represented as UserSystem.Linux.
    /// </summary>
    public static UserSystem System { get; internal set; }

    internal static void Initialize()
    {
        System = UserSystem.Unknown;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            System = UserSystem.Windows;

            try
            {
                IntPtr ntdllHandle = GetModuleHandle("ntdll.dll");

                if (ntdllHandle != IntPtr.Zero)
                {
                    IntPtr wineVersionProc = GetProcAddress(ntdllHandle, "wine_get_version");

                    if (wineVersionProc != IntPtr.Zero)
                        System = UserSystem.Linux;
                }
            }
            catch { }
        }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
}

/// <summary>
/// An operating system environment.
/// </summary>
public enum UserSystem
{
    /// <summary>
    /// Unknown operating system.
    /// </summary>
    Unknown = -1,

    /// <summary>
    /// Windows NT
    /// </summary>
    Windows,

    /// <summary>
    /// The user is on Linux. This is set when the game is running under Wine or Proton.
    /// </summary>
    Linux
};