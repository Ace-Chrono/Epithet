using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBoss : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject deathMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Paused");
            pauseMenu.SetActive(true);
            Dialogue.instance.SetTyperStarter(false);
            Stopwatch.instance.setTimerPause(true);
            PlayerHealth.instance.setBossPause();
            Dialogue.instance.setMonologuePause();
            deathMenu.SetActive(false);
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        if (Dialogue.instance.getGameStarted() == true)
            Dialogue.instance.SetTyperStarter(true);
        if (PlayerHealth.instance.returnPlayerHealth() <= 0)
            deathMenu.SetActive(true);
        Stopwatch.instance.setTimerPause(false);
        PlayerHealth.instance.breakBossPause();
        Dialogue.instance.breakMonologuePause();
    }

    public void ToMainMenu()
    {
        Dialogue.instance.GameMusic.Stop();
        MusicControl.instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
