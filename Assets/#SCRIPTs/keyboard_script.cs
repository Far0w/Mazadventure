using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyboard_script : MonoBehaviour
{
    public string typedText;
    public Text typedTextGO;
    public field selectedField; // If there are multiple fields in the scene

    private string[] keys = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "_", "Enter", "Clear", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"};
    private GameObject[] keysGO; // Keys GameObject, those that are white, that appears when the pointer hovering a key 
    private GameObject kbColliders;
    private int activeKey; // Index of the active touch in the keys array

    void Start()
    {
        string keyToDisable;
        kbColliders = GameObject.Find("keyboardColliders");
        keysGO = new GameObject[keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            keyToDisable = keys[i] + "_key";
            keysGO[i] = GameObject.Find(keyToDisable);
            keysGO[i].SetActive(false);
        }
        typedText = "";
        updateText();
        StartCoroutine(clearEntireKeys());
    }


    private void updateText()
    {
        typedTextGO.text = typedText;
    }

    public void onKey(string keyName)
    {
        //print("onKey + " + keyName);
        int id = System.Array.IndexOf(keys, keyName);
        keysGO[id].SetActive(true);
        activeKey = id;

    }

    public void clearKeys()
    {
        
        keysGO[activeKey].SetActive(false);
    }

    public IEnumerator clearEntireKeys()
    {
        while (true)
        {
            string keyToDisable;
            for (int i = 0; i < keys.Length; i++)
            {
                keyToDisable = keys[i] + "_key";
                if (i != activeKey)
                    keysGO[i].SetActive(false);
            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    public void closeKeyboard()
    {
        // Disabling canvas and colliders associated to keys
        kbColliders.SetActive(false);
        transform.GetComponent<Canvas>().enabled = false;
        transform.GetComponent<CanvasScaler>().enabled = false;
        transform.GetComponent<GraphicRaycaster>().enabled = false;
    }

    public void openKeyboard()
    {
        // Enabling canvas and colliders associated to keys
        kbColliders.SetActive(true);
        transform.GetComponent<Canvas>().enabled = true;
        transform.GetComponent<CanvasScaler>().enabled = true;
        transform.GetComponent<GraphicRaycaster>().enabled = true;
    }

    public void clickOnKey(string keyName)
    {
        if(keyName.Length == 1 && typedText.Length <= 16) // Trigger a normal key touch (number, letter or _)
        {
            typedText += keyName;
            updateText();
        }
        if (keyName == "Clear")
        {
            typedText = "";
            updateText();
        }
        if (keyName == "Enter")
        {
            closeKeyboard();
            if (selectedField != null)
                selectedField.enterText(typedText);
            typedText = "";
            updateText();
        }
    }

}
