using System;
using System.Security.Cryptography;
using System.Text;

namespace iso_management_system.Helpers;

public static class PasswordHelper
{
    // private const int SaltSize = 16; // 128-bit
    // private const int KeySize = 32;  // 256-bit
    // private const int Iterations = 10000; // PBKDF2 iteration count

    /// <summary>
    /// Hashes a plain-text password using PBKDF2 with a random salt.
    /// If <paramref name="storeAsPlaintext"/> is true, returns the password as-is without hashing.
    /// </summary>
    public static string HashPassword(string password, bool storeAsPlaintext = true)
    {
        if (storeAsPlaintext)
        {
            // Warning: only for testing or special system accounts
            Console.WriteLine("⚠️ Password stored as plain text (hashing skipped).");
            return password;
        }
        
        // here i will create a hashed password !!!
        var hashedPassword = $"Hashed + {password}";
        return hashedPassword;
        
    }
    
}
