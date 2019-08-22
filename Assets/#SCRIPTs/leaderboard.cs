using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class leaderboard : MonoBehaviour
{

    public Text scoreText;
    public Text pageText;

    public string leaderboardDifficulty = "";

    private string[] playerNames;
    private string[] scores;
    private int numberOfScoresByPage = 10;
    private int numberOfScores;
    private int numberOfAvailablePages = 0;
    private int currentPage = 0;

    private const string dreamloPublicURL_Dev    = "http://dreamlo.com/lb/5d5828dee6a81b07f0bfa67a/pipe";
    private const string dreamloPublicURL_Easy   = "";
    private const string dreamloPublicURL_Hard   = "";
    private const string dreamloPublicURL_Expert = "";

    private string dreamloPublicURL;

    private void Start()
    {
        dreamloPublicURL = dreamloPublicURL_Dev;
        switch (leaderboardDifficulty)
        {
            case "easy":
                dreamloPublicURL = dreamloPublicURL_Easy;
                break;
            case "hard":
                dreamloPublicURL = dreamloPublicURL_Hard;
                break;
            case "expert":
                dreamloPublicURL = dreamloPublicURL_Expert;
                break;
        }
        StartCoroutine(getHighscores(true));
    }



    public void changePage(string colliderName)
    {
        if (colliderName == "collider_next" && numberOfAvailablePages > currentPage + 1)
        {
            currentPage += 1;
        }
        else if (colliderName == "collider_previous" && currentPage - 1 >= 0)
        {
            currentPage -= 1;
        }
        showScores(currentPage);
    }




    IEnumerator getHighscores(bool updateUI) // if updateUI == 0 , UI as scoreText will not be update (use when gettingInformation without using the leaderboard, like getting the position of the player in the leaderboard)
    {
        if(updateUI)
            scoreText.text = "DOWNLOADING . . .";

        UnityWebRequest request = UnityWebRequest.Get(dreamloPublicURL);
        yield return request.SendWebRequest();

        if ((request.isHttpError || request.isNetworkError) && updateUI)
        {
            scoreText.text = "ERROR WITH DOWNLOADING SCORES";
        }
            
        else
        {
            getInfosFromHighscoreText(request.downloadHandler.text);
        }

        yield return null;

        if(updateUI)
            showScores(currentPage);

        yield return null;
    }

    void getInfosFromHighscoreText(string highscoreText)
    {
        string[] scoresLines;
        string hText = highscoreText;

        scoresLines = hText.Split('\n');

        playerNames = new string[scoresLines.Length - 1];  // playersName[i] got the score scores[i] and he is the i-th players of the leaderboard
        scores      = new string[scoresLines.Length - 1];

        for (int i = 0; i < scoresLines.Length -1; i++) // length - 1 : bc scoresLines[scoresLines.Length-1] = ""
        {
            string[] line = scoresLines[i].Split('|');
            playerNames[i] = line[0];
            scores[i] = line[1];
        }
    }

    void showScores(int showedPage)
    {
        scoreText.text = "";
        numberOfScores = playerNames.Length;
        numberOfAvailablePages = Mathf.CeilToInt(numberOfScores * 1.0f / numberOfScoresByPage);
        for (int i = 0; i < Mathf.Min(numberOfScores - numberOfScoresByPage* showedPage , 10); i++)
        {
            scoreText.text += (i + numberOfScoresByPage * showedPage + 1).ToString() + ". " + playerNames[i + numberOfScoresByPage * showedPage] + " : " + scores[i + numberOfScoresByPage * showedPage];
            scoreText.text += '\n';
        }
        pageText.text = "Page " + (showedPage + 1).ToString() + " of " + numberOfAvailablePages.ToString();
    }

    int getPlayerLeaderboardPosition(string playerName)
    {
        if (playerNames.Length == 0)
        {
            return -1;
        }
        else
        {
            return (System.Array.IndexOf(playerNames, playerName) +1);
        }
        //return -2;
    }

}
