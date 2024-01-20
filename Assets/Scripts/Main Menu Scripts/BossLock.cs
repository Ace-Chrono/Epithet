using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossLock : MonoBehaviour
{
    public TextMeshProUGUI RoninText;
    public TextMeshProUGUI NumenText;
    public Button RoninButton;
    public Button NumenButton; 

    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("BossesBeat", 0)); 
        if (PlayerPrefs.GetInt("BossesBeat", 0) < 1)
        {
            RoninButton.interactable = false; 
            RoninText.text = "LOCKED"; 
        }
        if (PlayerPrefs.GetInt("BossesBeat", 0) < 2)
        {
            NumenButton.interactable = false;
            NumenText.text = "LOCKED";
        }
    }
}
