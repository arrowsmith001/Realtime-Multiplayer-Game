using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV; 
    public GameObject myCharacter;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    [PunRPC] void ChangeColour(int whichColour){
        //PlayerInfo.inst.allCharacter[whichCharacter]
    }
}
