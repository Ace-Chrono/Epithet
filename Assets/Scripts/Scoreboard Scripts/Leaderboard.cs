using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public TextMeshProUGUI entry_1;
    public TextMeshProUGUI entry_2;
    public TextMeshProUGUI entry_3;
    public TextMeshProUGUI entry_4;
    public TextMeshProUGUI entry_5;
    public int highscoreNum;

    string highscoreTableString = "highscoreTable";

    public bool isPoints; 

    private class Highscores
    {
        public List<int> highscoreList;
    }

    private void Awake()
    {
        highscoreTableString += highscoreNum;
        if (PlayerPrefs.HasKey(highscoreTableString) == false)
        {
                entry_1.text = "1. NONE";
                entry_2.text = "2. NONE";
                entry_3.text = "3. NONE";
                entry_4.text = "4. NONE";
                entry_5.text = "5. NONE";
        }
        else if (PlayerPrefs.HasKey(highscoreTableString) == true)
        {
            string jsonString = PlayerPrefs.GetString(highscoreTableString);
            Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
            if (isPoints == true)
            {
                if (highscores.highscoreList[0] == 1000)
                    entry_1.text = "1. NONE";
                else
                    entry_1.text = "1. " + highscores.highscoreList[0] + " SECONDS";

                if (highscores.highscoreList[1] == 1000)
                    entry_2.text = "2. NONE";
                else
                    entry_2.text = "2. " + highscores.highscoreList[1] + " SECONDS";

                if (highscores.highscoreList[2] == 1000)
                    entry_3.text = "3. NONE";
                else
                    entry_3.text = "3. " + highscores.highscoreList[2] + " SECONDS";

                if (highscores.highscoreList[3] == 1000)
                    entry_4.text = "4. NONE";
                else
                    entry_4.text = "4. " + highscores.highscoreList[3] + " SECONDS";

                if (highscores.highscoreList[4] == 1000)
                    entry_5.text = "5. NONE";
                else
                    entry_5.text = "5. " + highscores.highscoreList[4] + " SECONDS";
            }
            else
            {
                if (highscores.highscoreList[0] == 0)
                    entry_1.text = "1. NONE";
                else
                    entry_1.text = "1. " + highscores.highscoreList[0];

                if (highscores.highscoreList[1] == 0)
                    entry_2.text = "2. NONE";
                else
                    entry_2.text = "2. " + highscores.highscoreList[1];

                if (highscores.highscoreList[2] == 0)
                    entry_3.text = "3. NONE";
                else
                    entry_3.text = "3. " + highscores.highscoreList[2];

                if (highscores.highscoreList[3] == 0)
                    entry_4.text = "4. NONE";
                else
                    entry_4.text = "4. " + highscores.highscoreList[3];

                if (highscores.highscoreList[4] == 0)
                    entry_5.text = "5. NONE";
                else
                    entry_5.text = "5. " + highscores.highscoreList[4];
            }
        }
    }
}
