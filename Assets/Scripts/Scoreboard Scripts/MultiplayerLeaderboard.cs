using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LootLocker.Requests;

public class MultiplayerLeaderboard : MonoBehaviour
{
    public TextMeshProUGUI entry_1;
    public TextMeshProUGUI entry_2;
    public TextMeshProUGUI entry_3;
    public TextMeshProUGUI entry_4;
    public TextMeshProUGUI entry_5;

    const string privateCode = "7cPmQRZm9EGldzYTYT3HTwnfsHnbBlOUO1Mok3P7XP4g";
    const string publicCode = "63cd4ecb8f40bb08f4759256";
    const string webURL = "http://dreamlo.com/lb/";
    public Highscore[] highscoresList;

    string leaderboardKey = "globalHighscore";
    int leaderboardID = 10864;

    private void Awake()
    {
        //DownloadHighscores();
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return FetchTopHighscoresRoutine();
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
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

    public IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardKey, 5, 0, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] members = response.items;
                done = true;
                if (members.Length >= 5)
                {
                    entry_1.text = "1. " + members[0].score + " SECONDS, PLAYER: " + members[0].player.id;
                    entry_2.text = "2. " + members[1].score + " SECONDS, PLAYER: " + members[1].player.id;
                    entry_3.text = "3. " + members[2].score + " SECONDS, PLAYER: " + members[2].player.id;
                    entry_4.text = "4. " + members[3].score + " SECONDS, PLAYER: " + members[3].player.id;
                    entry_5.text = "5. " + members[4].score + " SECONDS, PLAYER: " + members[4].player.id;
                }
                else if (members.Length == 4)
                {
                    entry_1.text = "1. " + members[0].score + " SECONDS, PLAYER: " + members[0].player.id;
                    entry_2.text = "2. " + members[1].score + " SECONDS, PLAYER: " + members[1].player.id;
                    entry_3.text = "3. " + members[2].score + " SECONDS, PLAYER: " + members[2].player.id;
                    entry_4.text = "4. " + members[3].score + " SECONDS, PLAYER: " + members[3].player.id;
                    entry_5.text = "5. NONE";
                }
                else if (members.Length == 3)
                {
                    entry_1.text = "1. " + members[0].score + " SECONDS, PLAYER: " + members[0].player.id;
                    entry_2.text = "2. " + members[1].score + " SECONDS, PLAYER: " + members[1].player.id;
                    entry_3.text = "3. " + members[2].score + " SECONDS, PLAYER: " + members[2].player.id;
                    entry_4.text = "4. NONE";
                    entry_5.text = "5. NONE";
                }
                else if (members.Length == 2)
                {
                    entry_1.text = "1. " + members[0].score + " SECONDS, PLAYER: " + members[0].player.id;
                    entry_2.text = "2. " + members[1].score + " SECONDS, PLAYER: " + members[1].player.id;
                    entry_3.text = "3. NONE";
                    entry_4.text = "4. NONE";
                    entry_5.text = "5. NONE";
                }
                else if (members.Length == 1)
                {
                    entry_1.text = "1. " + members[0].score + " SECONDS, PLAYER: " + members[0].player.id;
                    entry_2.text = "2. NONE";
                    entry_3.text = "3. NONE";
                    entry_4.text = "4. NONE";
                    entry_5.text = "5. NONE";
                }
                else if (members.Length == 0)
                {
                    entry_1.text = "1. NONE";
                    entry_2.text = "2. NONE";
                    entry_3.text = "3. NONE";
                    entry_4.text = "4. NONE";
                    entry_5.text = "5. NONE";
                }
                Debug.Log("Assigned Highscores");
            }
            else
            {
                Debug.Log("Failed " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
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

        if (highscoresList.Length >= 5)
        {
            entry_1.text = "1. " + highscoresList[highscoresList.Length - 1].score + " SECONDS";
            entry_2.text = "2. " + highscoresList[highscoresList.Length - 2].score + " SECONDS";
            entry_3.text = "3. " + highscoresList[highscoresList.Length - 3].score + " SECONDS";
            entry_4.text = "4. " + highscoresList[highscoresList.Length - 4].score + " SECONDS";
            entry_5.text = "5. " + highscoresList[highscoresList.Length - 5].score + " SECONDS";
        }
        else if (highscoresList.Length == 4)
        {
            entry_1.text = "1. " + highscoresList[highscoresList.Length - 1].score + " SECONDS";
            entry_2.text = "2. " + highscoresList[highscoresList.Length - 2].score + " SECONDS";
            entry_3.text = "3. " + highscoresList[highscoresList.Length - 3].score + " SECONDS";
            entry_4.text = "4. " + highscoresList[highscoresList.Length - 4].score + " SECONDS";
            entry_5.text = "5. NONE";
        }
        else if (highscoresList.Length == 3)
        {
            entry_1.text = "1. " + highscoresList[highscoresList.Length - 1].score + " SECONDS";
            entry_2.text = "2. " + highscoresList[highscoresList.Length - 2].score + " SECONDS";
            entry_3.text = "3. " + highscoresList[highscoresList.Length - 3].score + " SECONDS";
            entry_4.text = "4. NONE";
            entry_5.text = "5. NONE";
        }
        else if (highscoresList.Length == 2)
        {
            entry_1.text = "1. " + highscoresList[highscoresList.Length - 1].score + " SECONDS";
            entry_2.text = "2. " + highscoresList[highscoresList.Length - 2].score + " SECONDS";
            entry_3.text = "3. NONE";
            entry_4.text = "4. NONE";
            entry_5.text = "5. NONE";
        }
        else if (highscoresList.Length == 1)
        {
            entry_1.text = "1. " + highscoresList[highscoresList.Length - 1].score + " SECONDS";
            entry_2.text = "2. NONE";
            entry_3.text = "3. NONE";
            entry_4.text = "4. NONE";
            entry_5.text = "5. NONE";
        }
        else if (highscoresList.Length == 0)
        {
            entry_1.text = "1. NONE";
            entry_2.text = "2. NONE";
            entry_3.text = "3. NONE";
            entry_4.text = "4. NONE";
            entry_5.text = "5. NONE";
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
}
