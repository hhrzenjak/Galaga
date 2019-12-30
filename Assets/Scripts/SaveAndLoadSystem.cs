using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public static  class SaveAndLoadSystem
{

    private static readonly string _path = Application.persistentDataPath + "/Highscore.csv";

    public static List<ScoreEntry> LoadScoreEntries()
    {

        List<ScoreEntry> entries = new List<ScoreEntry>();

        if (File.Exists(_path) == false)
        {

            for (int counter = 0; counter < Constants.EntriesCount; counter++)
                entries.Add(new ScoreEntry());

            SaveScoreEntries(entries);
            return entries;

        }

        StreamReader reader = new StreamReader(_path);

        string[] data = reader.ReadToEnd().Split('\n');

        for (int counter = 0; counter < data.Length; counter++)
        {

            if (data[counter].Equals(""))
                continue;

            entries.Add(new ScoreEntry(data[counter].Split(',')[0], Int32.Parse(data[counter].Split(',')[1])));

        }

        reader.Close();
        return entries;

    }

    public static void ResetScoreEntries()
    {
        File.Delete(_path);
        LoadScoreEntries();
    }

    public static void SaveScoreEntries(List<ScoreEntry> entries) 
    {

        if (File.Exists(_path) == false)
            File.Create(_path).Close();

        StreamWriter writer = new StreamWriter(_path, false);

        foreach (ScoreEntry entry in entries)
            writer.WriteLine(entry.Name + "," + entry.Score);


        writer.Close();

    }

}
