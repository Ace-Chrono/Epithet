using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static Timer instance; 

    [SerializeField]
    private TextMeshProUGUI timerText;
    public int countdownTime;
    public int TimeReducePeriod;
    public int newTimeAdded;
    public bool timerPause = false;

    private void Awake()
    {
        instance = this;
    }

    public void StartCountdownToEnd()
    {
        StartCoroutine(CountdownToEnd());
        StartCoroutine(AddTimeReducer()); 
    }

    public IEnumerator CountdownToEnd()
    {
        while (countdownTime > 0)
        {
            while (timerPause == true)
            {
                yield return null;
            }

            timerText.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }
        
        if (ScoreManager.instance.NewHighscore() == true)
        {
            ScoreManager.instance.AddHighscoreEntry();
        }
        else
        {
            ScoreManager.instance.AddOtherScore();
        }

        Countdown.instance.SetTyperStarter(false);
        Countdown.instance.GameMusic.Stop(); 
        MusicControl.instance.GetComponent<AudioSource>().Play();
        TransitionTriggers.instance.LoadTrigger(1, 5);
    }

    public IEnumerator AddTimeReducer()
    {
        while (TimeReducePeriod > 0)
        {
            yield return new WaitForSeconds(1f);

            TimeReducePeriod--;
        }

        Typer.instance.timeAdded = newTimeAdded; 
    }

    public void addTime(int time)
    {
        countdownTime += time; 
    }

    public void setTimerPause(bool a)
    {
        timerPause = a;
    }
}
