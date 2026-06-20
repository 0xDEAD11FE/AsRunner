using System.Runtime.InteropServices;
using System.Text;
using WinApiWrapper.Models;

namespace WinApiWrapper;

public class CredentialManager
{
    private const string TrayPrefix = "Tray:";

    /// <summary>Ключ хранения учётных данных приложения: "Tray:{domain}\{userName}".</summary>
    public static string TrayTargetName(string domain, string userName) => $"{TrayPrefix}{domain}\\{userName}";

    public static string? GetPassword(string targetName)
    {
        if (Methods.CredRead(targetName, 1, 0, out IntPtr credPtr)) // 1 = Generic Credential
        {
            try
            {
                var cred = Marshal.PtrToStructure<CREDENTIAL>(credPtr);
                byte[] passwordBytes = new byte[cred.CredentialBlobSize];
                Marshal.Copy(cred.CredentialBlob, passwordBytes, 0, (int)cred.CredentialBlobSize);
                return Encoding.Unicode.GetString(passwordBytes);
            }
            finally
            {
                Methods.CredFree(credPtr);
            }
        }

        return null; // Пароль не найден
    }

    public static void SavePassword(string targetName, string userName, string password)
    {
        var cred = new CREDENTIAL
        {
            Type = 1, // Generic
            TargetName = targetName,
            UserName = userName,
            Persist = 2 // Local Machine (сохранять после перезагрузки)
        };

        byte[] bPassword = Encoding.Unicode.GetBytes(password);
        cred.CredentialBlobSize = (uint)bPassword.Length;
        cred.CredentialBlob = Marshal.AllocCoTaskMem(bPassword.Length);
        Marshal.Copy(bPassword, 0, cred.CredentialBlob, bPassword.Length);

        try
        {
            if (!Methods.CredWrite(ref cred, 0))
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        finally
        {
            Marshal.FreeCoTaskMem(cred.CredentialBlob);
        }
    }

    public static List<(string targetName, string userName, string domain)> GetTrayCredentials()
    {
        var result = new List<(string targetName, string userName, string domain)>();

        if (!Methods.CredEnumerate(null, 0, out int count, out IntPtr credentials))
            return result;

        try
        {
            for (int i = 0; i < count; i++)
            {
                IntPtr credPtr = Marshal.ReadIntPtr(credentials, i * IntPtr.Size);
                var cred = Marshal.PtrToStructure<CREDENTIAL>(credPtr);

                if (cred.TargetName?.StartsWith(TrayPrefix) == true)
                {
                    // Формат: "Tray:Domain\UserName"
                    string key = cred.TargetName.Substring(TrayPrefix.Length);
                    string[] parts = key.Split('\\');
                    string domain = parts.Length > 0 ? parts[0] : string.Empty;
                    string userName = parts.Length > 1 ? parts[1] : string.Empty;

                    result.Add((cred.TargetName, userName, domain));
                }
            }
        }
        finally
        {
            Methods.CredFree(credentials);
        }

        return result;
    }

    public static bool DeleteCredential(string targetName)
    {
        return Methods.CredDelete(targetName, 1, 0); // 1 = Generic Credential
    }
}
