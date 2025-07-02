using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public static class AbbrevationUtility
{
    private static readonly SortedDictionary<long, string> abbrevations = new SortedDictionary<long, string>
     {
         {100000,"K"},
         {10000000, "M" },
         //{1000000000, "B" }
     };

    private static readonly SortedDictionary<string, string> colours = new SortedDictionary<string, string>
    {
        {"K", "#F6FF00FF" },
        {"M", "#10D50CFF" }
    };


    public static string AbbreviateNumber(this int number, bool useColour = true)
    {
        //manual check; if the number is less than the first abbreviation, return it as a string
        //this should save computations
        if (number < abbrevations.First().Key) return number.ToString();

        for (int i = abbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<long, string> pair = abbrevations.ElementAt(i);
            if (Mathf.Abs(number) >= pair.Key)
            {
                string colour = string.Empty;
                double roundedNumber = (number / (pair.Key / 100));
                string s = Math.Truncate(roundedNumber).ToString();

                if (useColour)
                {
                    colours.TryGetValue(pair.Value, out colour);
                    return "<color=" + colour + ">" + s + pair.Value + "</color>";
                }
                else
                {
                    return s + pair.Value;
                }
            }
        }
        //catch all other cases, return the number as a string
        return number.ToString();
    }
    /*
    public static int UnAbbreviateNumber(this string number)
    {

    }
    */
}