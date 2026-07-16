using System.Runtime.InteropServices;
using WinApiWrapper.Models;

namespace WinApiWrapper;

public class ProcessLauncher
{
    public static void LaunchNetOnly(
        string path,
        string domain,
        string user,
        string password,
        string? workingDirectory = null,
        string? arguments = null)
    {
        var si = new STARTUPINFO();
        si.cb = Marshal.SizeOf(si);

        // Без аргументов оставляем командную строку как есть (историческое поведение).
        // С аргументами путь берём в кавычки, чтобы пробелы не ломали разбор.
        string commandLine = string.IsNullOrEmpty(arguments) ? path : $"\"{path}\" {arguments}";

        bool success = Methods.CreateProcessWithLogonW(
            user,
            domain,
            password,
            Constants.LogonFlags.NetCredentialsOnly,
            null,               // ApplicationName (можно null, если путь в commandLine)
            commandLine,        // CommandLine
            0,                  // CreationFlags
            IntPtr.Zero,        // Environment
            workingDirectory,   // CurrentDirectory (null → как у родителя)
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
