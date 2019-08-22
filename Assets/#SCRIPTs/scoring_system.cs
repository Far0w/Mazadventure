using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scoring_system : MonoBehaviour
{
    public Text timerText;
    public Canvas canva;
    public string finalTimeStr;
    public string playerName;
    public string difficulty;

    private float startTime;
    private GameObject rightHand;
    private float time;
    private int mins;
    private int secs;
    private bool isOver = false;
    private float finalTime;

    IEnumerator Start()
    {
        if (SceneManager.GetActiveScene().name == "game")
        {
            timerText.text = "XX:XX";

            yield return new WaitForSeconds(1);
            startTime = Time.time;
            StartCoroutine(timer());
            rightHand = GameObject.Find("RightControllerAnchor");

            canva.transform.parent = rightHand.transform;
            canva.transform.localPosition = Vector3.zero;
            canva.transform.localRotation = Quaternion.identity;

            playerName = GameObject.FindGameObjectWithTag("Player").transform.name;

            difficulty = GameObject.Find("menuCapsule").GetComponent<menu_capsule>().difficulty;

        }

        else if (!isOver) // Si on le gameObject est là just pour test
        {
            isOver = true;
            if (finalTimeStr == "")
            {
                finalTime = 61;
                mins = 1;
                secs = 1;
                finalTimeStr = doTimerText(mins, secs);
            }
            
        }

        DontDestroyOnLoad(this.gameObject);


    }



    IEnumerator timer()
    {
        while (!isOver)
        {
            time = Time.time - startTime;
            mins = (int)(time / 60);
            secs = (int)(time % 60);
            timerText.text = doTimerText(mins, secs);
            yield return new WaitForSeconds(1);
        }
    }

    string doTimerText(int min, int sec) //Retourne 06:05 avec 6,5 en param
    {
        string returnedText = "";
        if (min < 10)
        {
            returnedText += "0";
        }
        returnedText += min.ToString();
        returnedText += ":";
        if (sec < 10)
        {
            returnedText += "0";
        }
        returnedText += sec.ToString();
        return returnedText;
    }

    void stopTimer()
    {
        isOver = true;
        finalTimeStr = doTimerText(mins, secs);
    }
}
