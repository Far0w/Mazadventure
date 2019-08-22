using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNameField_script : MonoBehaviour
{
    // S'occupe de la partie récupération du nom du joueur

    private field fieldPN;
    private string fieldText = "";
    private menu_capsule menuCapsule;

    void Start()
    {
        fieldPN = gameObject.GetComponent<field>();
        fieldText = fieldPN.text;
        menuCapsule = GameObject.Find("menuCapsule").GetComponent<menu_capsule>();

        // Search if a playerNameCapsule is existing (if the player already played and played a game)
        if (GameObject.Find("playerName_capsule") != null)
        {
            fieldPN.enterText(GameObject.Find("playerName_capsule").GetComponent<playerNameCapsule>().playerName);
        }
        else
        {
            print("No playerName capsule found");
        }
        Destroy(GameObject.Find("playerName_capsule"));
    }


    void fieldUpdated() // This function is executed when the field is updated via a SendMessage
    {
        fieldText = fieldPN.text;
        menuCapsule.updatePlayerName(fieldText);
    }
}
