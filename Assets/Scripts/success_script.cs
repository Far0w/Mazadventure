using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class success_script : MonoBehaviour
{

    private Text successMess;
    private Text playerNameText;
    public scoring_system scoreSyst;

    IEnumerator Start()


    {
        
        yield return new WaitForSeconds(0.1f); // On attend une frame pour être sûr que scoreSyst all Ok
        playerNameText = GameObject.Find("playerNameMessage").GetComponent<Text>();
        


        successMess = GameObject.Find("SucessMessage").GetComponent<Text>();
        scoreSyst = GameObject.Find("scoringSystem").GetComponent<scoring_system>();

        playerNameText.text = scoreSyst.playerName;

        //print("scoreSyst.finalTimeStr" + scoreSyst.finalTimeStr);

        successMess.text = successMess.text + scoreSyst.finalTimeStr + " !";

        Destroy(scoreSyst);

        yield return new WaitForSeconds(7);

        SceneManager.LoadScene(0);
    }

}


