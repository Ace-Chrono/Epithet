using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerMultiplayer : MonoBehaviour
{
    public static TimerMultiplayer instance;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private int timerTime = 0;

    private bool gameEnded = false;

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
        timerText.gameObject.SetActive(true); 

        while (gameEnded == false)
        {
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
}
