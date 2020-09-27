using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public Transform[] spawnPoints;
    public static GameSetup inst;
    // Start is called before the first frame update
    void Awake()
    {
        if(GameSetup.inst == null){
            GameSetup.inst = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
      //   
    }
}
