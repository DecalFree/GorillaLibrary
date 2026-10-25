using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GorillaLibrary.Utilities;

/// <summary>
/// Utilities for system tasks.
/// </summary>
public static class SystemUtility
{
    /// <summary>
    /// The operating system the environment is running on.
    /// Note that this also includes detection for Wine. To get proper OS-dependent pathing, use SystemUtility.GetFolderPath.
    /// </summary>
    public static UserSystem System { get; internal set; }

    /// <inheritdoc cref="Environment.GetFolderPath(Environment.SpecialFolder)"/>
    /// <remarks>
    /// This substitute for Environment.GetFolderPath is designed to also work on Linux systems (UserSystem.Linux).
    /// </remarks>
    public static string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option = Environment.SpecialFolderOption.None)
    {
        if (System == UserSystem.Windows)
            return Environment.GetFolderPath(folder, option);

        // Binx note: this does not properly implement native GetFolderPath exceptions
        // (ArgumentException / PlatformNotSupportedException)

        string folderPath = string.Empty;

        switch (folder)
        {
            case Environment.SpecialFolder.UserProfile:
                folderPath = UserFolder;
                break;
            case Environment.SpecialFolder.Desktop:
                folderPath = Path.Combine(UserFolder, "Desktop");
                break;
            case Environment.SpecialFolder.MyDocuments:
                folderPath = Path.Combine(UserFolder, "Documents");
                break;
            case Environment.SpecialFolder.ApplicationData:
                folderPath = Path.Combine(UserFolder, ".config");
                break;
            case Environment.SpecialFolder.LocalApplicationData:
                folderPath = Path.Combine(UserFolder, ".local", "share");
                break;
            case Environment.SpecialFolder.CommonApplicationData:
                folderPath = Path.Combine(RootFolder, "usr", "share");
                break;
            case Environment.SpecialFolder.MyPictures:
                folderPath = Path.Combine(UserFolder, "Pictures");
                break;
            case Environment.SpecialFolder.MyMusic:
                folderPath = Path.Combine(UserFolder, "Music");
                break;
            case Environment.SpecialFolder.MyVideos:
                folderPath = Path.Combine(UserFolder, "Videos");
                break;
            case Environment.SpecialFolder.Templates:
                folderPath = Path.Combine(UserFolder, "Templates");
                break;
            case Environment.SpecialFolder.CommonTemplates:
                folderPath = Path.Combine(RootFolder, "usr", "share", "Templates");
                break;
            case Environment.SpecialFolder.Fonts:
                folderPath = Path.Combine(UserFolder, ".local", "share", "Fonts");
                break;
            default:
                return "";
        }

        if (string.IsNullOrEmpty(folderPath))
            return string.Empty;

        if (option == Environment.SpecialFolderOption.Create && !Directory.Exists(folderPath))
        {
            try { Directory.CreateDirectory(folderPath); }
            catch (UnauthorizedAccessException) { return string.Empty; }
        }

        return folderPath;
    }

    private static string UserFolder
    {
        get
        {
            if (!string.IsNullOrEmpty(field))
                return field;

            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (System != UserSystem.Linux)
                return userProfile;

            field = Path.Combine("Z:\\home", new DirectoryInfo(userProfile).Name);

            return field;
        }
    }

    private static string RootFolder
    {
        get
        {
            if (string.IsNullOrEmpty(field))
                field = System == UserSystem.Linux ? "Z:" : "C:";

            return field;
        }
    }

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