using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo inst;
    public int playerColorSelection = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if(PlayerInfo.inst == null){
            PlayerInfo.inst = this;
        }else{
            if(PlayerInfo.inst != this){
                Destroy(PlayerInfo.inst.gameObject);
                PlayerInfo.inst = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
