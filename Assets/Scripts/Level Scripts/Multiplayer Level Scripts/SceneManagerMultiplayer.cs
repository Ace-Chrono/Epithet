using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class SceneManagerMultiplayer : MonoBehaviour
{
    public static SceneManagerMultiplayer instance;

    public int countdownTime;
    [SerializeField]
    private TextMeshProUGUI countdownText;
    public AudioSource GameMusic;

    private bool TyperStarter = false;

    int playerCount;

    bool gameStarted = false;
    bool hasLeft = false;
    bool gameEnded = false;

    int highscore;
    [SerializeField]
    private TextMeshProUGUI highscoreText;
    const string privateCode = "7cPmQRZm9EGldzYTYT3HTwnfsHnbBlOUO1Mok3P7XP4g";
    const string publicCode = "63cd4ecb8f40bb08f4759256";
    const string webURL = "http://dreamlo.com/lb/";
    public Highscore[] highscoresList;

    public GameObject timer;
    public GameObject mainMenuButton;

    string leaderboardKey = "globalHighscore";
    int leaderboardID = 10864;

    void Update()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        if (playerCount == 2 && gameStarted == false)
        {
            mainMenuButton.SetActive(false);
            StartCoroutine(CountdownToStart());
            gameStarted = true; 
        }
        if (gameStarted == true && playerCount == 1 && hasLeft == false || gameEnded == true && hasLeft == false)
        {
            hasLeft = true;
            timer.SetActive(false);
            highscoreText.gameObject.SetActive(false);
            TimerMultiplayer.instance.setGameEnded(true); 
            SetTyperStarter(false);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            GameMusic.Stop();
            MusicControl.instance.GetComponent<AudioSource>().Play();
            TransitionTriggers.instance.LoadTrigger(1, 23);
        }
    }

    public void startCountdown()
    {
        StartCoroutine(CountdownToStart());
    }

    private void Awake()
    {
        instance = this;
        MusicControl.instance.GetComponent<AudioSource>().Stop();
        GameMusic.Play();
    }

    private void Start()
    {
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return FetchTopHighscoreRoutine();
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        //Logs in a user as a guest through lootlocker
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            //tests if the login was successful or not
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopHighscoreRoutine()
    {
        bool done = false;
        //Gets the leaderboard from lootlocker and takes the first value (the second parameter)
        LootLockerSDKManager.GetScoreList(leaderboardKey, 1, 0, (response) =>
        {
            //Checks if the request was successful
            if (response.success)
            {
                LootLockerLeaderboardMember[] members = response.items;
                done = true;
                //Assigns the text to the highscore if there is a highscore
                if (members.Length == 1)
                {
                    highscore = members[0].score;
                    highscoreText.text = "GLOBAL HIGHSCORE: " + highscore.ToString() + " SECONDS";
                }
                else
                {
                    highscore = 100000000;
                    highscoreText.text = "HIGHSCORE: NONE";
                }
                Debug.Log("Assigned Highscore");
            }
            else
            {
                Debug.Log("Failed " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        countdownText.text = "TYPE!";

        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);

        StartGame();
    }

    public void DownloadHighscores()
    {
        StartCoroutine("DownloadHighscoresFromDatabase");
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
            FormatHighscores(www.text);
        else
            Debug.Log("error downloading: " + www.error);

        if (highscoresList.Length >= 1)
        {
            highscore = highscoresList[highscoresList.Length - 1].score;
            highscoreText.text = "HIGHSCORE: " + highscore.ToString() + " SECONDS";
        }
        else
        {
            highscore = 1000;
            highscoreText.text = "HIGHSCORE: NONE";
        }

    }

    void FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresList = new Highscore[entries.Length];
        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highscoresList[i] = new Highscore(username, score);
            print(highscoresList[i].username + ": " + highscoresList[i].score);
        }
    }

    public struct Highscore
    {
        public string username;
        public int score;

        public Highscore(string _username, int _score)
        {
            username = _username;
            score = _score;
        }
    }

    

    public int getHighscore()
    {
        return highscore; 
    }

    void StartGame()
    {
        timer.SetActive(true);
        TimerMultiplayer.instance.StartTimerToEnd();
        TyperStarter = true;
        highscoreText.gameObject.SetActive(true);
    }

    public void SetTyperStarter(bool a)
    {
        TyperStarter = a;
    }

    public bool GetTyperStarter()
    {
        return TyperStarter;
    }

    public void setGameEnded(bool a)
    {
        gameEnded = a; 
    }

    public bool returnGameEnded()
    {
        return gameEnded; 
    }

    public bool returnGameStarted()
    {
        return gameStarted;
    }
}
