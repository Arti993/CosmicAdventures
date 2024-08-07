using System.Collections.Generic;
using UnityEngine;

public class GameProgress : IGameProgress
{
    private const string Level1ResultTag = "Level1Result";
    private const string Level2ResultTag = "Level2Result";
    private const string Level3ResultTag = "Level3Result";
    private const string Level4ResultTag = "Level4Result";
    private const string Level5ResultTag = "Level5Result";
    private const string Level6ResultTag = "Level6Result";
    private const string Level7ResultTag = "Level7Result";
    private const string Level8ResultTag = "Level8Result";
    private const string Level9ResultTag = "Level9Result";
    private const string Level10ResultTag = "Level10Result";
    private const string Level11ResultTag = "Level11Result";
    private const string Level12ResultTag = "Level12Result";

    private Dictionary<int, string> _levelResultsTagsDictionary = new Dictionary<int, string>();
    
    public GameProgress()
    {
        string[] levelsResultsTags = new string[]
        {
            Level1ResultTag,Level2ResultTag,Level3ResultTag,Level4ResultTag,Level5ResultTag,
            Level6ResultTag,Level7ResultTag,Level8ResultTag,Level9ResultTag,Level10ResultTag,
            Level11ResultTag, Level12ResultTag
        };

        int levelNumber;
        
        for (int i = 0; i < levelsResultsTags.Length; i++)
        {
            levelNumber = i + 1;
            _levelResultsTagsDictionary.Add(levelNumber, levelsResultsTags[i]);
        }
    }

    public void SaveLevelProgress(int points)
    {
        int levelNumber = GetCurrentLevelNumber();

        _levelResultsTagsDictionary.TryGetValue(levelNumber, out string levelResultTag);

        if (PlayerPrefs.GetInt(levelResultTag, 0) < points)
        {
            PlayerPrefs.SetInt(levelResultTag, points);
            UnlockNextLevelIfNeed();
        }
    }
    
    
    public int GetCurrentLevelNumber()
    {
        int levelNumber = PlayerPrefs.GetInt("SelectedLevelNumber", 0);

        if (levelNumber == 0)
            levelNumber = PlayerPrefs.GetInt("ReachedLevelNumber", 1);

        return levelNumber;
    }

    public int GetLevelResult(int levelNumber)
    {
        _levelResultsTagsDictionary.TryGetValue(levelNumber, out string levelResultTag);

        return PlayerPrefs.GetInt(levelResultTag, 0);
    }

    public int GetTotalScore()
    {
        int totalScore = 0;
        
        for (int i = 0; i < _levelResultsTagsDictionary.Count; i++)
        {
            totalScore += GetLevelResult(i + 1);
        }

        return totalScore;
    }

    public void ClearSaves()
    {
        foreach (var level in _levelResultsTagsDictionary)
        {
            PlayerPrefs.SetInt(level.Value, 0);
        }
        
        PlayerPrefs.SetInt("ReachedLevelNumber", 1);
    }

    public void SelectLevel(int levelNumber)
    {
        PlayerPrefs.SetInt("SelectedLevelNumber", levelNumber);
    }

    private void UnlockNextLevelIfNeed()
    {
        int completedLevelNumber = GetCurrentLevelNumber();
        
        int reachedLevelNumber = PlayerPrefs.GetInt("ReachedLevelNumber", 1);

        if (completedLevelNumber == reachedLevelNumber)
        {
            reachedLevelNumber++;
            
            PlayerPrefs.SetInt("ReachedLevelNumber", reachedLevelNumber);
        }
    }
}
