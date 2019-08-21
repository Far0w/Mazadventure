using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exit_script : MonoBehaviour
{

    private GameObject scoreSyst;

    private void Start()
    {
        
        scoreSyst = GameObject.Find("scoringSystem");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") //au cas où
        {
            scoreSyst.SendMessage("stopTimer");
            SceneManager.LoadScene("Scenes/sucess_scene");

        }
    }

}
