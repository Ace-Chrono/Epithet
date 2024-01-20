using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PauseMenuMultiplayer : MonoBehaviour
{
    public GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Paused");
            pauseMenu.SetActive(true);
            SceneManagerMultiplayer.instance.SetTyperStarter(false);
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        if (SceneManagerMultiplayer.instance.returnGameStarted() == true)
            SceneManagerMultiplayer.instance.SetTyperStarter(true);
    }

    public void ToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManagerMultiplayer.instance.GameMusic.Stop();
        MusicControl.instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
