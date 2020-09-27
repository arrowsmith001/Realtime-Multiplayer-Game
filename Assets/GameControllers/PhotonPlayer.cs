using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PhotonPlayer : MonoBehaviour
{
    public PhotonPlayer instance;

    private PhotonView PV;
    public GameObject myAvatar;

    private Color avatarColor;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();        

        if(PV.IsMine){
            myAvatar = 
                PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PhotonPlayerAvatar"),
                GameSetup.inst.spawnPoints[Random.Range(0, GameSetup.inst.spawnPoints.Length)].position,
                Quaternion.identity);
        }

        //MenuController.inst.colorChangeEvent.AddListener(OnColorChanged);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
