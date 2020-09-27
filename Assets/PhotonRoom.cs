using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom inst;
    private PhotonView PV;
    // Start is called before the first frame update
    void Awake()
    {
        if(PhotonRoom.inst == null){
            PhotonRoom.inst = this;
        }else if(PhotonRoom.inst != this){
            Destroy(PhotonRoom.inst.gameObject);
            PhotonRoom.inst = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        PV = GetComponent<PhotonView>();
    }

    public override void OnEnable() {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }
    public override void OnDisable() {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public int currentScene;
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode){
        Debug.Log("Scene loaded");
        currentScene = scene.buildIndex;

        CreatePlayer();

        Debug.Log("Scene loaded");
    }

    private void CreatePlayer(){
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable changedProps){
        
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps){
        
        GameObject avatar = GetAvatarFromPlayer(target);
        SetPropertiesOfAvatar(avatar, changedProps);
    }

    private void SetPropertiesOfAvatar(GameObject avatar, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(avatar == null) return;
        if(changedProps.ContainsKey("color")) 
        {
            avatar.GetComponent<Controller>().ChangeColor(MenuController.inst.GetColor((int) changedProps["color"]));
        }
    }

    public GameObject GetAvatarFromPlayer(Player player){
        foreach (GameObject avatar in GameObject.FindGameObjectsWithTag("Avatar")) {
            Player p = avatar.GetComponent<PhotonView>().Owner;
            if(p == player) return avatar;
        }

        return null;
    }

    Player[] photonPlayers;
    int playersInRoom;
    int myNumberInRoom;

    public override void OnJoinedRoom(){
        Debug.Log("Room joined");
        base.OnJoinedRoom();
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

    }


    public void LeaveRoom(){
      //  PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom(){
        SceneManager.LoadScene(0);
    } 


    // Update is called once per frame
    void Update()
    {
        
    }
}
