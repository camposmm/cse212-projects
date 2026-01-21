using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character
    /// words (lower case, no duplicates). Using sets, find an O(n)
    /// solution for returning all symmetric pairs of words.
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        // Encode each 2-char word into an int for very fast HashSet lookups.
        // code = (firstChar << 16) | secondChar
        static int Encode(string w) => (w[0] << 16) | w[1];

        var set = new HashSet<int>(words.Length);

        // O(n): build the set
        foreach (var w in words)
        {
            set.Add(Encode(w));
        }

        var results = new List<string>();

        // O(n): check for symmetric pairs
        foreach (var w in words)
        {
            // Special case: "aa" should not match anything
            if (w[0] == w[1]) continue;

            int code = Encode(w);
            int revCode = (w[1] << 16) | w[0];

            if (set.Contains(revCode))
            {
                // Avoid duplicates: only output once (canonical ordering)
                if (code < revCode)
                {
                    string reversed = new string(new[] { w[1], w[0] });
                    results.Add($"{w} & {reversed}");
                }
            }
        }

        return results.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();

        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(',');

            // 4th column -> index 3 (0-based)
            if (fields.Length < 4) continue;

            var degree = fields[3].Trim();

            if (!degrees.ContainsKey(degree))
            {
                degrees[degree] = 1;
            }
            else
            {
                degrees[degree] += 1;
            }
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a
    /// new word.  A dictionary is used to solve the problem.
    ///
    /// Important Note: ignore spaces and ignore case.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        string w1 = word1.Replace(" ", "").ToLowerInvariant();
        string w2 = word2.Replace(" ", "").ToLowerInvariant();

        if (w1.Length != w2.Length) return false;

        var counts = new Dictionary<char, int>();

        foreach (var ch in w1)
        {
            if (!counts.ContainsKey(ch)) counts[ch] = 0;
            counts[ch] += 1;
        }

        foreach (var ch in w2)
        {
            if (!counts.ContainsKey(ch)) return false;

            counts[ch] -= 1;
            if (counts[ch] < 0) return false;
        }

        // If all counts are zero, it's an anagram
        foreach (var kvp in counts)
        {
            if (kvp.Value != 0) return false;
        }

        return true;
    }

    /// <summary>
    /// Reads USGS earthquake JSON for the current day and returns formatted strings:
    /// "Place - Mag X"
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";

        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);

        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        if (featureCollection?.Features == null || featureCollection.Features.Count == 0)
            return Array.Empty<string>();

        var results = new List<string>();

        foreach (var feature in featureCollection.Features)
        {
            var place = feature?.Properties?.Place;
            var mag = feature?.Properties?.Mag;

            if (string.IsNullOrWhiteSpace(place) || mag is null)
                continue;

            // Use invariant formatting so decimals are consistent
            results.Add($"{place} - Mag {mag.Value.ToString(CultureInfo.InvariantCulture)}");
        }

        return results.ToArray();
    }
}