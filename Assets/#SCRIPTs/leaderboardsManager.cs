using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class leaderboardsManager : MonoBehaviour
{
    public GameObject leaderboardGO;
    public Image easy_Image;
    public Image hard_Image;
    public Image expert_Image;

    private GameObject currentLeaderboard;


    private string chosenDifficulty = "Easy";


    private IEnumerator Start()
    {
        instantiateLeaderboard("Easy");
        yield return null;
        /*yield return new WaitForSeconds(6);
        loadNewLeaderboard("collider_diff_hard");*/
    }


    private void instantiateLeaderboard(string difficulty)
    {
        changeButtonColor(chosenDifficulty, false);
        chosenDifficulty = difficulty;
        changeButtonColor(chosenDifficulty, true);
        currentLeaderboard = Instantiate(leaderboardGO);
        currentLeaderboard.transform.parent = transform;
        currentLeaderboard.transform.localPosition = new Vector3(0, 0.28f, 0);
        currentLeaderboard.GetComponent<leaderboard>().leaderboardDifficulty = difficulty;
    }

    private void changeButtonColor(string difficulty_Button , bool phase)
        //phase = true : selected button ; = false : released button
    {
        Color selectedColor = new Vector4(0.47451f, 0.2666f, 0.1647f, 0.784f);
        Color releasedColor = new Vector4(0.349f  , 0.1765f, 0.09f  , 0.784f);

        Image ImageToModify = easy_Image;

        switch (difficulty_Button.ToLower())
        {
            case "easy":
                ImageToModify = easy_Image;
                break;
            case "hard":
                ImageToModify = hard_Image;
                break;
            case "expert":
                ImageToModify = expert_Image;
                break;
        }

        if (phase)
        {
            ImageToModify.color = selectedColor;
        }
        else
        {
            ImageToModify.color = releasedColor;
        }
    }

    public void loadNewLeaderboard(string nameOfCollider)
    {
        Destroy(currentLeaderboard);
        instantiateLeaderboard(nameOfCollider.Split('_')[2]);
    }


    public void changePage(string page)
    {
        currentLeaderboard.GetComponent<leaderboard>().changePage(page);
    }
}
