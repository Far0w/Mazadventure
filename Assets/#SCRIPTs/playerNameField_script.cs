using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNameField_script : MonoBehaviour
{

    private field fieldPN;
    private string fieldText = "";
    private menu_capsule menuCapsule;

    void Start()
    {
        fieldPN = gameObject.GetComponent<field>();
        fieldText = fieldPN.text;
        menuCapsule = GameObject.Find("menuCapsule").GetComponent<menu_capsule>();
    }


    void fieldUpdated() // This function is executed when the field is updated via a SendMessage
    {
        fieldText = fieldPN.text;
        menuCapsule.updatePlayerName(fieldText);
    }
}
