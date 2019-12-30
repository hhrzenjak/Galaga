using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DataManager : MonoBehaviour
{

    #region DATA MANAGER PROPERTY

    private static DataManager _DM;

    public static DataManager DM
    {
        get
        {
            if (_DM == null)
                _DM = FindObjectOfType<DataManager>();
            return _DM;
        }
    }

    #endregion

    private List<ScoreEntry> _entries = new List<ScoreEntry>();

    private void Awake()
    {

        #region DATA MANAGER PROPERTY SET-UP

        if (_DM == null)
            _DM = this;

        if (_DM.Equals(this) == false)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        #endregion

        LoadScoreEntries();

    }

    #region SAVE AND LOAD SCORE ENTRIES METHODS

    private void LoadScoreEntries()
    {
        _entries = SaveAndLoadSystem.LoadScoreEntries();
    }

    private void ResetScoreEntries()
    {
        SaveAndLoadSystem.ResetScoreEntries();
        LoadScoreEntries();
    }

    private void SaveScoreEntries()
    {
        SaveAndLoadSystem.SaveScoreEntries(_entries);
    }

    #endregion

    private void UpdateScoreEntries()
    {

        _entries.Sort((entryOne, entryTwo) => entryOne.CompareTo(entryTwo));

        while (_entries.Count > Constants.EntriesCount)
            _entries.RemoveAt(_entries.Count - 1);

        SaveScoreEntries();

    }

    public int AddScoreEntry(string name, int score)
    {

        if (score < _entries[_entries.Count - 1].Score)
            return -1;

        _entries.Add(new ScoreEntry(name, score));

        UpdateScoreEntries();
        return _entries.IndexOf(new ScoreEntry(name, score));

    }

    public ScoreEntry[] GetTopEntries(int number)
    {

        ScoreEntry[] entries = new ScoreEntry[number];

        for (int counter = 0; counter < number; counter++)
            entries[counter] = _entries[counter];

        return entries;

    }

    public ScoreEntry GetEntry(int index)
    {

        if (index >= _entries.Count)
            return null;

        return _entries[index];

    }

}
