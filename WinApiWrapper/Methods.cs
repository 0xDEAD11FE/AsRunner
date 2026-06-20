using System.Runtime.InteropServices;
using WinApiWrapper.Models;

namespace WinApiWrapper;

internal static class Methods
{
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern bool CreateProcessWithLogonW(
                string lpUsername,
                string lpDomain,
                string lpPassword,
                uint dwLogonFlags,
                string? lpApplicationName,
                string lpCommandLine,
                uint dwCreationFlags,
                nint lpEnvironment,
                string? lpCurrentDirectory,
                ref STARTUPINFO lpStartupInfo,
                out PROCESS_INFORMATION lpProcessInformation);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool CloseHandle(IntPtr hObject);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern bool CredRead(string target, uint type, uint reserved, out IntPtr credentialPtr);

    [DllImport("advapi32.dll", SetLastError = true)]
    internal static extern void CredFree(IntPtr credentialPtr);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern bool CredWrite([In] ref CREDENTIAL userCredential, [In] uint flags);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern bool CredEnumerate(string? filter, uint flags, out int count, out IntPtr credentials);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern bool CredDelete(string target, uint type, uint flags);
}