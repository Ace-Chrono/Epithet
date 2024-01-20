using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu2 : MonoBehaviour
{
    public GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Paused");
            pauseMenu.SetActive(true);
            Countdown2.instance.SetTyperStarter(false);
            Countdown2.instance.stopCountdown();
            Timer2.instance.setTimerPause(true);
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Countdown2.instance.SetTyperStarter(true);
        Countdown2.instance.restartCountdown();
        Timer2.instance.setTimerPause(false);
    }

    public void ToMainMenu()
    {
        Countdown2.instance.GameMusic.Stop();
        MusicControl.instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
