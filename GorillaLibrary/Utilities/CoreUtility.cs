using GorillaNetworking;
using System;
using System.Runtime.InteropServices;
using static SteamVR_Utils;

namespace GorillaLibrary.Utilities;

public static class CoreUtility
{
    public static bool Initialized { get; private set; }

    /// <summary>
    /// The platform the Gorilla Tag install is based on (either Oculus PC or Steam)
    /// </summary>
    public static string Platform { get; private set; }

    /// <summary>
    /// Whether the Gorilla Tag install is based on the Steam platform.
    /// </summary>
    public static bool IsSteam => Platform == "Steam";

    internal static void Initialize()
    {
        Initialized = true;
        Platform = PlayFabAuthenticator.instance.platform.ToString();

        SystemUtility.System = UserSystem.Unknown;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            SystemUtility.System = UserSystem.Windows;

            try
            {
                IntPtr ntdllHandle = GetModuleHandle("ntdll.dll");

                if (ntdllHandle != IntPtr.Zero)
                {
                    IntPtr wineVersionProc = GetProcAddress(ntdllHandle, "wine_get_version");

                    if (wineVersionProc != IntPtr.Zero)
                        SystemUtility.System = UserSystem.Linux;
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