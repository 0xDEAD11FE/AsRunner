using System.Runtime.InteropServices;
using System.Text;
using WinApiWrapper.Models;

namespace WinApiWrapper;

public class CredentialManager
{
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
}
