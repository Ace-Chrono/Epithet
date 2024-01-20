using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; 

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText; 
    public int highscoreNum; 

    int score = 0;
    int highscore = 0;
    int hsToAdd = 0;   

    string highscoreString = "highscore"; 
    bool newHighscore = false;
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
        highscore = PlayerPrefs.GetInt(highscoreString, 0); 
        scoreText.text = score.ToString() + " POINTS";
        if (highscore == 0)
            highscoreText.text = "HIGHSCORE: NONE";
        else
            highscoreText.text = "HIGHSCORE: " + highscore.ToString();
        highscoreTableString += highscoreNum; 
        if (PlayerPrefs.HasKey(highscoreTableString) == false)
        {
            List<int> highscoreListSetter = new List<int> { 0, 0, 0, 0, 0 };
            Highscores highscores = new Highscores { highscoreList = highscoreListSetter };
            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString(highscoreTableString, json);
        }
    }

    public void AddPoint(string word, int comboDecider)
    {
        //List based on 26 letters and their points
        int[] letterPoints = { 10, 30, 25, 15, 10, 25, 25, 15, 15, 30, 30, 25, 25, 15, 15, 30, 30, 15, 15, 10, 25, 30, 25, 30, 25, 30 };
        int points = 0;
        //gets each letter in the word, finds their order in alphabet and assigns points accordingly
        foreach (char c in word)
        {
            int letterNum = (int)c - 97;
            Debug.Log(c.ToString().ToUpper() + " " + letterNum + " " + letterPoints[letterNum]); 
            points += letterPoints[letterNum];
        }
        points += comboDecider; 
        score += points; 
        scoreText.text = score.ToString() + " POINTS"; 
        //Sets a newhighscore to add is the score is higher than the previous highscore
        if (highscore < score)
        {
            hsToAdd = score; 
            newHighscore = true;
        }
    }

    public void AddHighscoreEntry()
    {
        //Sets new highscore
        PlayerPrefs.SetInt(highscoreString, hsToAdd);
        //Gets list of previous highscores
        string jsonString = PlayerPrefs.GetString(highscoreTableString);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        //inserts new highscore into list
        highscores.highscoreList.Insert(0, hsToAdd);
        //removes last item in list
        highscores.highscoreList.RemoveAt(5);
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString(highscoreTableString, json);
        PlayerPrefs.Save(); 
    }

    public void AddOtherScore()
    {
        bool stopper = false; 
        //gets previous highscores
        string jsonString = PlayerPrefs.GetString(highscoreTableString);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        //compares the submitted score to see if it is higher than any of the previous highscores and inserts it in order
        for (int x = 1; x < 5; x++)
        {
            if (score > highscores.highscoreList[x] && stopper == false)
            {
                highscores.highscoreList.Insert(x, score);
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

    public int GetScore()
    {
        return score; 
    }

    public void RemovePoints(int x)
    {
        if (score >= x)
            score -= x;
        if (score < x)
            score = 0;
        scoreText.text = score.ToString() + " POINTS";
    }
}
