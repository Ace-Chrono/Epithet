using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionComplete : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public Button mainMenuButton;
    public Button mainMenuButton2;
    public Button nextBossButton;
    public TextMeshProUGUI nextBossButtonText;

    void Start()
    {
        //Check if these if statements work
        if (PlayerController.instance.getGameLost() == true)
            levelText.text = "YOU HAVE LOST";
        else if (PlayerController.instance.getGameLost() == false)
            levelText.text = "YOU HAVE WON!";

        if (ScoreController.instance.NewHighscore())
            scoreText.text = "NEW HIGHSCORE!" + "\nTIME: " + Stopwatch.instance.getTimerTime().ToString() + " SECONDS";
        else
            scoreText.text = "TIME: " + Stopwatch.instance.getTimerTime().ToString() + " SECONDS";
        TransitionTriggers.instance.LoadTrigger(2, 0);

        if (PlayerController.instance.numBoss == 1)
        {
            nextBossButtonText.text = "NEXT BOSS: RONIN";
        }
        else if (PlayerController.instance.numBoss == 2)
        {
            nextBossButtonText.text = "NEXT BOSS: NUMEN";
        }
        else if (PlayerController.instance.numBoss == 3)
        {
            nextBossButton.gameObject.SetActive(false);
            mainMenuButton.gameObject.SetActive(false);
            mainMenuButton2.gameObject.SetActive(true);
        }
    }

    public void nextBoss()
    {
        if (PlayerController.instance.numBoss == 1)
        {
            TransitionTriggers.instance.LoadTrigger(1, 18);
        }
        else if (PlayerController.instance.numBoss == 2)
        {
            TransitionTriggers.instance.LoadTrigger(1, 19);
        }
    }
} 

