using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class CreateAndJoinLobby : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public RoomDisplay roomDisplayPrefab;
    List<RoomDisplay> roomDisplayList = new List<RoomDisplay>();
    public Transform contentObject;
    public GameObject noRoomsMessage;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        if (createInput.text.Length >= 1)
            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
        if (roomList.Count > 0)
        {
            noRoomsMessage.SetActive(false);
        }
        else
        {
            noRoomsMessage.SetActive(true);
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomDisplay item in roomDisplayList)
        {
            Destroy(item.gameObject);
            Debug.Log("Room Destroyed");
        }
        roomDisplayList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomDisplay newRoom = Instantiate(roomDisplayPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomDisplayList.Add(newRoom);
            Debug.Log("Room Instatiated");

        }

        Debug.Log("Room Count: " + list.Count);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(22);
    }
}
