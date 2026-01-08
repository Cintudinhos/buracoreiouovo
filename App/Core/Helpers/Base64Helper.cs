using System.Text;

namespace App.Core.Helpers;

public static class Base64Helper
{
    public static string FromBase64(string base64)
    {
        int remainder = base64.Length % 4;
        if (remainder != 0)
        {
            base64 = remainder switch
            {
                2 => $"{base64}==",
                3 => $"{base64}=",
                _ => throw new ArgumentException("Invalid base 64 size", nameof(base64)),
            };
        }

        base64 = base64.Replace('-', '+').Replace('_', '/');
        byte[] base64Bytes = Convert.FromBase64String(base64);

        return Encoding.UTF8.GetString(base64Bytes);
    }
}
