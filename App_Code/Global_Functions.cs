using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Global_Functions
/// </summary>
public class Global_Functions
{
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

}