using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LevelTransitions : MonoBehaviour
{
    private List<int> nums = new List<int>() {2, 3, 4, 5, 9, 11, 14, 16, 18, 19};

    public void loadScene(int num) 
    {
        bool triggerTransition = false; 
        for (int x = 0; x < nums.Count; x++)
        {
            if (nums[x] == num)
            {
                triggerTransition = true; 
            }
        }
        if (triggerTransition == true)
            TransitionTriggers.instance.LoadTrigger(1, num); 
        else 
            SceneManager.LoadScene(num);
    }

    public void disconnectNetworkAndLoad(int num)
    {
        PhotonNetwork.Disconnect();
        bool triggerTransition = false;
        for (int x = 0; x < nums.Count; x++)
        {
            if (nums[x] == num)
            {
                triggerTransition = true;
            }
        }
        if (triggerTransition == true)
            TransitionTriggers.instance.LoadTrigger(1, num);
        else
            SceneManager.LoadScene(num);
    }

    public void disconnectNetworkRoomAndLoad(int num)
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        bool triggerTransition = false;
        for (int x = 0; x < nums.Count; x++)
        {
            if (nums[x] == num)
            {
                triggerTransition = true;
            }
        }
        if (triggerTransition == true)
            TransitionTriggers.instance.LoadTrigger(1, num);
        else
            SceneManager.LoadScene(num);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit(); 
    }

    public void playMusic()
    {
        MusicControl.instance.GetComponent<AudioSource>().Play();
    }
}
