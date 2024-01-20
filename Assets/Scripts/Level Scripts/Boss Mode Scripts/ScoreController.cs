using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static ScoreController instance;

    public int highscoreNum;
    private int highscore = 0;
    private int hsToAdd = 0;

    private string highscoreString = "highscore";
    private bool newHighscore = false;
    string highscoreTableString = "highscoreTable";

    private class Highscores
    {
        public List<int> highscoreList;
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        highscoreString += highscoreNum.ToString();
        highscore = PlayerPrefs.GetInt(highscoreString, 1000);
        highscoreTableString += highscoreNum;
        if (PlayerPrefs.HasKey(highscoreTableString) == false)
        {
            List<int> highscoreListSetter = new List<int> { 1000, 1000, 1000, 1000, 1000 };
            Highscores highscores = new Highscores { highscoreList = highscoreListSetter };
            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString(highscoreTableString, json);
        }
    }

    public void AddHighscoreEntry()
    {
        PlayerPrefs.SetInt(highscoreString, hsToAdd);
        string jsonString = PlayerPrefs.GetString(highscoreTableString);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        highscores.highscoreList.Insert(0, hsToAdd);
        highscores.highscoreList.RemoveAt(5);
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString(highscoreTableString, json);
        PlayerPrefs.Save();
    }

    public void AddOtherScore()
    {
        bool stopper = false;
        string jsonString = PlayerPrefs.GetString(highscoreTableString);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        for (int x = 1; x < 5; x++)
        {
            if (Stopwatch.instance.getTimerTime() < highscores.highscoreList[x] && stopper == false)
            {
                highscores.highscoreList.Insert(x, Stopwatch.instance.getTimerTime());
                highscores.highscoreList.RemoveAt(5);
                stopper = true;
            }
        }
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString(highscoreTableString, json);
        PlayerPrefs.Save();
    }

    public bool NewHighscore()
    {
        return newHighscore;
    }

    public void setNewHighscore(bool x)
    {
        newHighscore = x;
    }

    public void SetHsToAdd(int x)
    {
        hsToAdd = x;
    }

    public string getHighscoreString()
    {
        return highscoreString;
    }
}
