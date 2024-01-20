using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using LootLocker.Requests;

public class P1ScoreManager : MonoBehaviour
{
    public static P1ScoreManager instance;

    public TextMeshProUGUI scoreText;

    private int score = 0;
    private bool newHighscore = false;

    PhotonView view;

    const string privateCode = "7cPmQRZm9EGldzYTYT3HTwnfsHnbBlOUO1Mok3P7XP4g";
    const string publicCode = "63cd4ecb8f40bb08f4759256";
    const string webURL = "http://dreamlo.com/lb/";

    string leaderboardKey = "globalHighscore";
    int leaderboardID = 10864; 

    private class Highscores
    {
        public List<int> highscoreList;
    }

    private void Awake()
    {
        instance = this;
        view = GetComponent<PhotonView>();
    }

    void Start()
    {
        scoreText.text = score.ToString() + "/" + P1Typer.instance.pointGoal + " POINTS";
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
        syncScore(points);
        if (score >= P1Typer.instance.pointGoal)
        {
            syncScoreText(P1Typer.instance.pointGoal + "/" + P1Typer.instance.pointGoal + " POINTS");
            SubmitScore(TimerMultiplayer.instance.getTimerTime());
        }
        else
        {
            syncScoreText(score.ToString() + "/" + P1Typer.instance.pointGoal + " POINTS");
        }
    }

    public void syncScore(int t)
    {
        view.RPC("updateScore", RpcTarget.All, t);
    }

    [PunRPC]
    void updateScore(int t)
    {
        score += t; 
    }

    public void syncScoreText(string t)
    {
        view.RPC("updateScoreText", RpcTarget.All, t);
    }

    [PunRPC]
    void updateScoreText(string t)
    {
        scoreText.text = t.ToUpper();
        UnityEngine.Debug.Log(t.ToUpper());
    }

    public void SubmitScore(int score)
    {
        StartCoroutine(SubmitScoreRoutine(score));
    }

    IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        //Uploads the score using the player id
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardKey, (response) =>
        {
            //Checks if upload was successful
            if (response.success)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("failed " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);

        //Last three lines are RPC calls to end the game
        if (scoreToUpload < SceneManagerMultiplayer.instance.getHighscore())
        {
            syncNewHighscore();
        }
        P1Typer.instance.syncWinStatus();
        P1Typer.instance.syncGameEnded();
    }

    public void syncNewHighscore()
    {
        view.RPC("newHighscoreSync", RpcTarget.All);
    }

    [PunRPC]
    void newHighscoreSync()
    {
        newHighscore = true;
    }

    public void AddNewHighscore(string username, int score)
    {
        StartCoroutine(UploadNewHighscore(username, score)); 
    }

    IEnumerator UploadNewHighscore(string username, int score)
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
            Debug.Log("upload successful");
        else
            Debug.Log("error uploading" + www.error);

        if (score < SceneManagerMultiplayer.instance.getHighscore())
        {
            syncNewHighscore();
        }

        P1Typer.instance.syncWinStatus();
        P1Typer.instance.syncGameEnded();
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
        syncScoreText(score.ToString() + "/" + P1Typer.instance.pointGoal + " POINTS");
    }
}
