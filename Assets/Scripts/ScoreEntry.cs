using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ScoreEntry
{

    #region NAME PROPERTY

    private string _name;

    public string Name
    {
        get
        {
            return _name;
        }
    }

    #endregion

    #region SCORE PROPERTY

    private int _score;

    public int Score
    {
        get
        {
            return _score;
        }
    }

    #endregion

    public ScoreEntry()
    {
        _name = "";
        _score = 0;
    }

    public ScoreEntry(string name, int score)
    {
        _name = name;
        _score = score;
    }

    public int CompareTo(ScoreEntry entry)
    {

        if (_score.CompareTo(entry.Score) != 0)
            return -_score.CompareTo(entry.Score);

        return _name.CompareTo(entry.Name);

    }

    public override bool Equals(object other)
    {

        if (GetType().Equals(other.GetType()) == false)
            return false;

        ScoreEntry entry = (ScoreEntry)other;

        return _name.Equals(entry.Name) && _score.Equals(entry.Score);

    }

    public override int GetHashCode()
    {
        return _name.GetHashCode() + _score.GetHashCode();
    }

}
