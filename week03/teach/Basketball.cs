using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

/*
 * CSE 212 Lesson 6C 
 * 
 * This code will analyze the NBA basketball data and create a table showing
 * the players with the top 10 career points.
 * 
 * Note about columns:
 * - Player ID is in column 0
 * - Points is in column 8
 * 
 * Each row represents the player's stats for a single season with a single team.
 */

public class Basketball
{
    public static void Run()
    {
        // Build a reliable path to basketball.csv that works no matter where you run dotnet from.
        var csvPath = Path.Combine(AppContext.BaseDirectory, "basketball.csv");

        // If it's not copied to output, try next to the project files (week03/teach/basketball.csv).
        if (!File.Exists(csvPath))
        {
            csvPath = Path.Combine(Directory.GetCurrentDirectory(), "week03", "teach", "basketball.csv");
        }

        if (!File.Exists(csvPath))
        {
            Console.WriteLine("ERROR: basketball.csv not found.");
            Console.WriteLine("Make sure basketball.csv is located at:");
            Console.WriteLine("  week03/teach/basketball.csv");
            Console.WriteLine("OR it must be copied to the build output folder.");
            return;
        }

        // Key = playerId, Value = total career points
        var players = new Dictionary<string, int>();

        using var reader = new TextFieldParser(csvPath);
        reader.TextFieldType = FieldType.Delimited;
        reader.SetDelimiters(",");
        reader.ReadFields(); // ignore header row

        while (!reader.EndOfData)
        {
            var fields = reader.ReadFields();
            if (fields is null || fields.Length < 9) continue;

            var playerId = fields[0];

            if (!int.TryParse(fields[8], out var points))
                continue;

            if (!players.ContainsKey(playerId))
                players[playerId] = 0;

            players[playerId] += points;
        }

        var top10 = players
            .OrderByDescending(kvp => kvp.Value)
            .Take(10)
            .ToArray();

        Console.WriteLine("Top 10 players by total career points (PlayerId: Points)");
        for (int i = 0; i < top10.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {top10[i].Key}: {top10[i].Value}");
        }
    }
}