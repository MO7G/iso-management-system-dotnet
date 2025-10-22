using System;
using System.Security.Cryptography;
using System.Text;

namespace iso_management_system.Helpers;

public static class PasswordHelper
{
    private const int SaltSize = 16; // 128-bit
    private const int KeySize = 32;  // 256-bit
    private const int Iterations = 10000; // PBKDF2 iteration count

    /// <summary>
    /// Hashes a plain-text password using PBKDF2 with a random salt.
    /// If <paramref name="storeAsPlaintext"/> is true, returns the password as-is without hashing.
    /// </summary>
    public static string HashPassword(string password, bool storeAsPlaintext = false)
    {
        if (storeAsPlaintext)
        {
            // Warning: only for testing or special system accounts
            Console.WriteLine("⚠️ Password stored as plain text (hashing skipped).");
            return password;
        }

        // Generate a random salt
        using var rng = RandomNumberGenerator.Create();
        byte[] salt = new byte[SaltSize];
        rng.GetBytes(salt);

        // Derive key using PBKDF2
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] key = pbkdf2.GetBytes(KeySize);

        // Combine salt and key
        var hashBytes = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(key, 0, hashBytes, SaltSize, KeySize);

        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Verifies whether the provided password matches the stored hash.
    /// </summary>
    public static bool VerifyPassword(string password, string storedHash)
    {
        // If stored as plaintext (no hashing was done)
        if (!IsBase64String(storedHash))
        {
            return password == storedHash;
        }

        var hashBytes = Convert.FromBase64String(storedHash);

        // Extract salt and key
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);
        byte[] storedKey = new byte[KeySize];
        Array.Copy(hashBytes, SaltSize, storedKey, 0, KeySize);

        // Derive key from input password
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] derivedKey = pbkdf2.GetBytes(KeySize);

        return CryptographicOperations.FixedTimeEquals(storedKey, derivedKey);
    }

    /// <summary>
    /// Checks if a string is valid Base64.
    /// Used to detect if stored password is hashed or plain.
    /// </summary>
    private static bool IsBase64String(string input)
    {
        Span<byte> buffer = new(new byte[input.Length]);
        return Convert.TryFromBase64String(input, buffer, out _);
    }
}
