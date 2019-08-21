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

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void selectLevel(string level)
    {
        Easy = false;
        Medium = false;
        Expert = false;
        Supereasy = false;

        switch (level)
        {
            case "easy":
                Easy = true;
                break;
            case "medium":
                Medium = true;
                break;
            case "expert":
                Expert = true;
                break;
            case "supereasy":
                Supereasy = true;
                break;
        }
    }

    public string getLevel()
    {
        if (Easy)
            return "easy";
        if (Medium)
            return "medium";
        if (Expert)
            return "expert";
        if (Supereasy)
            return "supereasy";
        return "";
    }



}
