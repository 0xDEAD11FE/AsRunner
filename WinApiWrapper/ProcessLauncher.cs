using System.Runtime.InteropServices;
using WinApiWrapper.Models;

namespace WinApiWrapper;

public class ProcessLauncher
{
    public static void LaunchNetOnly(string path, string domain, string user, string password)
    {
        var si = new STARTUPINFO();
        si.cb = Marshal.SizeOf(si);

        string commandLine = path;

        bool success = Methods.CreateProcessWithLogonW(
            user,
            domain,
            password,
            Constants.LOGON_NETCREDENTIALS_ONLY,
            null,           // ApplicationName (можно null, если путь в commandLine)
            commandLine,    // CommandLine
            0,              // CreationFlags
            IntPtr.Zero,    // Environment
            null,           // CurrentDirectory
            ref si,
            out var pi);

        if (!success)
        {
            int error = Marshal.GetLastWin32Error();
            throw new System.ComponentModel.Win32Exception(error);
        }

        // Дескрипторы процесса и потока нам не нужны — сразу закрываем,
        // иначе они утекают на каждый запуск.
        if (pi.hProcess != IntPtr.Zero)
            Methods.CloseHandle(pi.hProcess);
        if (pi.hThread != IntPtr.Zero)
            Methods.CloseHandle(pi.hThread);
    }
}
