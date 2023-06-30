using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject player;
    [Space]
    public Transform spwanPoint;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("connecting..");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("connected to server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("Falcons", null, null);
        Debug.Log("Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room Falcons");
        GameObject _player = PhotonNetwork.Instantiate(player.name, spwanPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().isLocalPlayer();

    }
}
