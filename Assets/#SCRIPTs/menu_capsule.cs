using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_capsule : MonoBehaviour
{
    //The capsule that sends datas from the menu to the game
    public string playerName = "";

    public string difficulty;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }



}
