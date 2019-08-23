using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu_capsule : MonoBehaviour
{
    //The capsule that sends datas from the menu to the game
    public string playerName = "";
    //public Text debugText5;

    public string difficulty;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void updatePlayerName(string pName)
    {
        playerName = pName;
        //debugText5.text = "menu_capsule : playerName updated to " + pName;
    }



}
