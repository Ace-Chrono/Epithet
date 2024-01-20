using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager2 : MonoBehaviour
{
    public static ScoreManager2 instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public int highscoreNum;

    private int score = 0;
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
        scoreText.text = score.ToString() + "/2000 POINTS";
        if (highscore == 1000)
            highscoreText.text = "HIGHSCORE: NONE";
        else 
            highscoreText.text = "HIGHSCORE: " + highscore.ToString() + " SECONDS";
        highscoreTableString += highscoreNum;
        if (PlayerPrefs.HasKey(highscoreTableString) == false)
        {
            List<int> highscoreListSetter = new List<int> { 1000, 1000, 1000, 1000, 1000 };
            Highscores highscores = new Highscores { highscoreList = highscoreListSetter };
            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString(highscoreTableString, json);
        }
    }

    public void AddPoint(string word, int comboDecider)
    {
        int[] letterPoints = { 10, 30, 25, 15, 10, 25, 25, 15, 15, 30, 30, 25, 25, 15, 15, 30, 30, 15, 15, 10, 25, 30, 25, 30, 25, 30 };
        int points = 0;
        foreach (char c in word)
        {
            int letterNum = (int)c - 97;
            Debug.Log(c.ToString().ToUpper() + " " + letterNum + " " + letterPoints[letterNum]);
            points += letterPoints[letterNum];
        }
        points += comboDecider;
        score += points;
        scoreText.text = score.ToString() + "/2000 POINTS";
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
            if (Timer2.instance.getTimerTime() < highscores.highscoreList[x] && stopper == false)
            {
                highscores.highscoreList.Insert(x, Timer2.instance.getTimerTime());
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

    public int GetScore()
    {
        return score;
    }

    public void SetHsToAdd(int x)
    {
        hsToAdd = x; 
    }

    public string getHighscoreString()
    {
        return highscoreString;
    }

    public void RemovePoints(int x)
    {
        if (score >= x)
            score -= x;
        if (score < x)
            score = 0;
        scoreText.text = score.ToString() + "/2000 POINTS";
    }
}
