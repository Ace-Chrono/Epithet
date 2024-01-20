using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Paused");
            pauseMenu.SetActive(true);
            Countdown.instance.SetTyperStarter(false);
            Countdown.instance.stopCountdown();
            Timer.instance.setTimerPause(true);
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Countdown.instance.SetTyperStarter(true);
        Countdown.instance.restartCountdown();
        Timer.instance.setTimerPause(false);
    }

    public void ToMainMenu()
    {
        Countdown.instance.GameMusic.Stop();
        MusicControl.instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
