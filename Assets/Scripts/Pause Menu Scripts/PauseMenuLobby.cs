using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PauseMenuLobby : MonoBehaviour
{
    public GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Paused");
            pauseMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }

    public void ToMainMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
