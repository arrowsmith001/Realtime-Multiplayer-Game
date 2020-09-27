using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] public class ColorChangeEvent : UnityEvent<int>{};

public class MenuController : MonoBehaviour
{
    public ColorChangeEvent colorChangeEvent = new ColorChangeEvent();

    public GameObject[] swatches;
    public static MenuController inst;

    private void Awake() {
        if(MenuController.inst == null){
            MenuController.inst = this;
        }else
        {
            if(MenuController.inst != this){
                Destroy(MenuController.inst.gameObject);
                MenuController.inst = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);

    }


    public void OnClickColorPick(int whichColor){
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        table.Add("color", whichColor);
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);
    }

    public Color GetColor(int i)
    {
        return swatches[i].GetComponent<Image>().color;
    }
}
