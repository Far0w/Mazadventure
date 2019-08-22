using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class field : MonoBehaviour
{
    public string text = "";
    public Text fieldText;
    public Image imageField;
    public Component fieldScript;
    public Text debugText4;


    public void enterText(string txt) //Dès que le text est entré par l'utilisateur via le clavier
    {

        text = txt;
        fieldText.text = txt;
        fieldScript.SendMessage("fieldUpdated");
        debugText4.text = "field.cs : Field updated, text = " + text;

    }

    public void isSelected()
    {
        imageField.color = new Vector4(0.8f,0.8f,0.8f,1f);
    }

    public void isUnselected()
    {
        imageField.color = new Vector4(1f, 1f, 1f, 1f);
    }
}
