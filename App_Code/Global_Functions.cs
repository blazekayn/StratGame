using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

/// <summary>
/// Summary description for Global_Functions
/// </summary>
public class Global_Functions
{
    /// <summary>
    /// Check if there is a user logged in. Ture=yes there is a user False=no there is not.
    /// </summary>
    /// <returns>is the user logged in</returns>
    public static bool CheckLoggedIn()
    {
        if (HttpContext.Current.Session["Player"] == null) { return false; } return true;
    }

    /// <summary>
    /// Replace the last occurance of a string in a string
    /// </summary>
    /// <param name="Source">starting string</param>
    /// <param name="Find">the string to find</param>
    /// <param name="Replace">the string to replace</param>
    /// <returns></returns>
    public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
    {
        int place = Source.LastIndexOf(Find);

        if (place == -1)
            return Source;

        string result = Source.Remove(place, Find.Length).Insert(place, Replace);
        return result;
    }

    /// <summary>
    /// Generates a salted hash. Used in password creation and validation
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
    {
        HashAlgorithm algorithm = new SHA256Managed();

        byte[] plainTextWithSaltBytes =
          new byte[plainText.Length + salt.Length];

        for (int i = 0; i < plainText.Length; i++)
        {
            plainTextWithSaltBytes[i] = plainText[i];
        }
        for (int i = 0; i < salt.Length; i++)
        {
            plainTextWithSaltBytes[plainText.Length + i] = salt[i];
        }

        return algorithm.ComputeHash(plainTextWithSaltBytes);
    }
    /// <summary>
    /// Compares two byte arrays. used for checking hashed passwords.
    /// </summary>
    /// <param name="array1"></param>
    /// <param name="array2"></param>
    /// <returns></returns>
    public static bool CompareByteArrays(byte[] array1, byte[] array2)
    {
        if (array1.Length != array2.Length)
        {
            return false;
        }

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
            {
                return false;
            }
        }

        return true;
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}