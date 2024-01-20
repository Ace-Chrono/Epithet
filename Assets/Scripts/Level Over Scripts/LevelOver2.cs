using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelOver2 : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject newHighscoreText;

    void Start()
    {
        scoreText.text = "SCORE: " + Timer2.instance.getTimerTime().ToString() + " SECONDS";
        if (ScoreManager2.instance.NewHighscore())
            newHighscoreText.SetActive(true);
        TransitionTriggers.instance.LoadTrigger(2, 0);
    }
}
