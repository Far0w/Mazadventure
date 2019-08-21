using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_capsule : MonoBehaviour
{
    //The capsule that sends datas from the menu to the game
    public string playerName = "";

    //Modifier getLevel si d'autres difficultés
    public bool Easy   = false;
    public bool Medium = false;
    public bool Expert = false;
    public bool Supereasy = false;

    public string difficulty;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }



}
