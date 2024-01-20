using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelOverMultiplayer : MonoBehaviour
{
    public TextMeshProUGUI p1StatusText;
    public TextMeshProUGUI p2StatusText;
    public TextMeshProUGUI noContestText;
    public TextMeshProUGUI newHighscoreText;

    void Start()
    {
        if (SceneManagerMultiplayer.instance.returnGameEnded() == true)
        {
            if (P1Typer.instance.returnWinStatus() == "WON")
            {
                p1StatusText.text = "PLAYER 1 " + P1Typer.instance.returnWinStatus() + "\nSCORE: " + P1Typer.instance.pointGoal + "/" + P1Typer.instance.pointGoal + " POINTS";
                p2StatusText.text = "PLAYER 2 " + P2Typer.instance.returnWinStatus() + "\nSCORE: " + P2ScoreManager.instance.GetScore() + "/" + P2Typer.instance.pointGoal + " POINTS";
            }
            else if (P2Typer.instance.returnWinStatus() == "WON")
            {
                p1StatusText.text = "PLAYER 1 " + P1Typer.instance.returnWinStatus() + "\nSCORE: " + P1ScoreManager.instance.GetScore() + "/" + P1Typer.instance.pointGoal + " POINTS";
                p2StatusText.text = "PLAYER 2 " + P2Typer.instance.returnWinStatus() + "\nSCORE: " + P2Typer.instance.pointGoal + "/" + P2Typer.instance.pointGoal + " POINTS";
            }
        }
        else if (P1Typer.instance.returnWinStatus() == "LOST" && P2Typer.instance.returnWinStatus() == "LOST")
        {
            noContestText.gameObject.SetActive(true); 
        }
        if (P1ScoreManager.instance.NewHighscore() || P2ScoreManager.instance.NewHighscore())
        {
            newHighscoreText.text = "NEW GLOBAL HIGHSCORE: " + TimerMultiplayer.instance.getTimerTime();
            newHighscoreText.gameObject.SetActive(true);
        }
        TransitionTriggers.instance.LoadTrigger(2, 0);
    }
}
