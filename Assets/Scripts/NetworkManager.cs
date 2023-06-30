using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject player;
    [Space]
    public Transform spwanPoint;
    public GameObject entryPanel;
    public GameObject existingPanel;
    public GameObject createRoomPanel;
    public TMP_InputField userName;
    public TMP_InputField roomName;
    string uName; // User name 
    string rName; // room name
    public Button loginButton;
    public Button createRoomButton;
    public Button existingRoomButton;
    public Button create;
    public Button cancel;
    public Button back;
    public Button joinRoom;
    private Dictionary<string, RoomInfo> roomListData;
    private Dictionary<string, GameObject> roomListDestroy;
    public GameObject roomItemPref;
    public GameObject roomItemParent;
    void Start()
    {
        roomListData = new Dictionary<string, RoomInfo>();
        roomListDestroy = new Dictionary<string, GameObject>();
    }
    void Update()
    {

    }
    public void OnClickLogin()
    {
        uName = userName.text;
        if (!string.IsNullOrEmpty(uName))
        {
            PhotonNetwork.LocalPlayer.NickName = uName;
            PhotonNetwork.ConnectUsingSettings();
            loginButton.gameObject.SetActive(false);
            userName.interactable = false;
            createRoomButton.gameObject.SetActive(true);
            existingRoomButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("name field cannot be empty");
        }
    }

    public void OnClickCreateRoom()
    {
        entryPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }

    public void OnClickExistingRoom()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        entryPanel.SetActive(false);
        existingPanel.SetActive(true);
    }

    public void OnClickCreate()
    {
        rName = roomName.text;
        RoomOptions rOptions = new RoomOptions();
        rOptions.MaxPlayers = 20;
        PhotonNetwork.CreateRoom(rName, rOptions);
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        createRoomPanel.SetActive(false);
        existingPanel.SetActive(true);
    }

    public void OnClickCancel()
    {
        createRoomPanel.SetActive(false);
        entryPanel.SetActive(true);
    }
    public void OnClickBack()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        createRoomPanel.SetActive(true);
        existingPanel.SetActive(false);
    }

    public void OnClickJoinRoom()
    {
        existingPanel.SetActive(false);
        GameObject _player = PhotonNetwork.Instantiate(PhotonNetwork.LocalPlayer.NickName, spwanPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().isLocalPlayer();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " you're connected to photon network");
        PhotonNetwork.JoinLobby();
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " room created");
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log(uName + " you're in the room " + rName);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomList();
        foreach (RoomInfo room in roomList)
        {
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (roomListData.ContainsKey(room.Name))
                {
                    roomListData.Remove(room.Name);
                }
            }
            else
            {
                if (roomListData.ContainsKey(room.Name))
                {
                    roomListData[room.Name] = room;
                }
                else
                {
                    roomListData.Add(room.Name, room);
                }
            }
        }
        foreach (RoomInfo roomItem in roomListData.Values)
        {
            GameObject roomListItemObject = Instantiate(roomItemPref);
            roomListItemObject.transform.SetParent(roomItemParent.transform);
            roomListItemObject.transform.localScale = Vector3.one;
            // updating values in prefab
            roomListItemObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = roomItem.Name;
            roomListItemObject.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => RoomJoinFromList(roomItem.Name));
            roomListDestroy.Add(roomItem.Name, roomListItemObject);
        }
    }
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        ClearRoomList();
        roomListData.Clear();
    }
    public void RoomJoinFromList(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(roomName);
    }
    public void ClearRoomList()
    {
        if (roomListDestroy.Count > 0)
        {
            foreach (var v in roomListDestroy.Values)
            {
                Destroy(v);
            }
            roomListDestroy.Clear();
        }
    }
}
