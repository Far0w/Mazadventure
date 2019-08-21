using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class menu_manager : MonoBehaviour
{
    // Fonctionnement du script :
    // Récolte les données : Quels boutons séléctionnés ...
    // En fonction des données modifie l'état des outons, passe au niveau suivant ..

    //Les différentes possibilités sont
    // baseMenu >  [ soloSelec  |  infos  | multiSelec  ]
    // infos > [ baseMenu ("back" button) ]
    // soloSelec > [ baseMenu(back) | soloCollec | soloRandom]

    public Color hoverColor;
    public GameObject[] canvasList;  //Dans le même ordre que possinleScenes
    public GameObject[] collidersList;

    public menu_capsule levelSelector; // GO donc le but est de savoir quel nv choisi par l'utilisateur pour permettre la génération de labyrinthe adaptée 

    public Text debugText1;
    public Text debugText2;

    public int currentScene = 0;
    private int lastScene   = 0; // Pour détecter si changement

    private string[] possibleScenes = { "baseMenu", "soloSelec", "infos", "soloRandom", "soloCollec" };
    private string currentPage;
    private bool isLevelSelected = false;
    private GameObject currentCanva = null;
    private GameObject currentCollider = null;

    private void Start()
    {
        isLevelSelected = false;
        changePage("baseMenu");
        debugText2.text = "MenuManager Init";
    }

    private void Update()
    {
        if (currentScene != lastScene)
        {
            changePage(possibleScenes[currentScene]);
            lastScene = currentScene;
        }
    }

    public void receiveData( string button ) 
        // actions possibles : "hover", "click"
    {
        debugText2.text = "CLOK";  
        if (button.StartsWith("selectLevel")){
            string levelReceived = (button.Substring(11)).ToLower(); // Enlève "selectLevel" puis met en minuscule
            levelSelector.selectLevel(levelReceived);
            isLevelSelected = true;
        }
        else if (button == "launch" && isLevelSelected)
        {
            SceneManager.LoadScene("Scenes/loading_scene");
        }
        else
        {
            changePage(button);
        }
        

    }

    void changePage(string newPage)
    {
        //Cas : initialisation
        if (currentCanva != null)
        {
            currentCanva.SetActive(false);
        }
        if(currentCollider != null)
        {
            currentCollider.SetActive(false);
        }


        //Cas : classique

        currentCanva = canvasList[stringSceneToIntScene(newPage)];
        currentCanva.SetActive(true);
        currentCollider = collidersList[stringSceneToIntScene(newPage)];
        currentCollider.SetActive(true);
        debugText1.text = newPage;
    }



    int stringSceneToIntScene(string Scene) //Pourrait opti
    {
        switch(Scene){
            case "baseMenu":
                return 0;
            case "soloSelec":
                return 1;
            case "infos":
                return 2;
            case "soloRandom":
                return 3;
            case "soloCollec":
                SceneManager.LoadScene("Scenes/loading_scene");
                break;
                //return 4;

        }
        return 0;
    }
}
