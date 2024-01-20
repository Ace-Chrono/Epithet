using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Stopwatch : MonoBehaviour
{
    public static Stopwatch instance;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private int timerTime = 0;

    private bool gameEnded = false;

    public bool timerPause = false;

    private void Awake()
    {
        instance = this;
    }

    public void StartTimerToEnd()
    {
        StartCoroutine(TimerToEnd());
    }

    public IEnumerator TimerToEnd()
    {
        while (gameEnded == false)
        {
            while (timerPause == true)
            {
                yield return null;
            }

            timerText.text = timerTime.ToString();

            yield return new WaitForSeconds(1f);

            timerTime++;
        }
    }

    public int getTimerTime()
    {
        return timerTime;
    }

    public void setGameEnded(bool x)
    {
        gameEnded = x;
    }

    public void setTimerPause(bool a)
    {
        timerPause = a;
    }
}
