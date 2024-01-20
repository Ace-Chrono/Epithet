using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelOverText : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject newHighscoreText; 

    void Start()
    {
        scoreText.text = "SCORE: " + ScoreManager.instance.GetScore().ToString();
        if (ScoreManager.instance.NewHighscore())
            newHighscoreText.SetActive(true);
        TransitionTriggers.instance.LoadTrigger(2, 0);
    }
}
