using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomDisplay : MonoBehaviourPun
{

    public TextMeshProUGUI roomName;
    CreateAndJoinLobby manager;

    private void Start()
    {
        manager = FindObjectOfType<CreateAndJoinLobby>();
    }

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void OnClickItem()
    {
        manager.JoinRoom(roomName.text);
    }
}
