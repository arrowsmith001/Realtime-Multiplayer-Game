using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] private int multiplayerSceneIndex; // Number for build index to multiplay scene

    public override void OnEnable(){
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable(){
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom(){
        Debug.Log("Joined room");
        StartGame();
    }

    private void StartGame(){

        if(PhotonNetwork.IsMasterClient){
            Debug.Log("Starting game...");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }

    }
}
