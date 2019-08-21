using UnityEngine;
using System.Collections;

public class maze_generator : MonoBehaviour {
    
    //PUBLIC VAR :
    public GameObject player;
    public string playerName = "playerName";

    public Material matGround;
    public Material matCeiling;
    public Material matWalls;
    public Material matBeams;

    public GameObject casesGO;
    public GameObject doorGO;
    public GameObject torchGO;

    public bool enableBeams = true;
    public int mazeSize = 10; //nb de cases sur un côté (labyrinthe carré)
    public int caseSize = 5; //taille d'une case en unité de longueur
    public float heightWall = 5f;
    public float wallThickeness = 1f;
    public float beamThickness = 1.1f;

    //PRIVATE VAR :
    private bool[,] etatCase; //Matrice des état des case : true = Déja passé
    private GameObject[,] cubeCase; // Matrice des gameObjects

    private float[] center = { 0, 0 };
    private bool isGenerated = false;
    private int compteur = 0; //Utilie pour la génération du labyrinthe et l'étiquettage des cases

    private menu_capsule menuCapsule;


    void Start () {
        // récupérer le niveau sélectionné par le joueur
        getLevelInfos();
        // Centre relatif : utile pour la construction des murs et des cases
        center[0] = (mazeSize * caseSize / 2);
        center[1] = (mazeSize * caseSize / 2);
		buildGround ();
		buildWalls();
        entryAndExit();
        if (enableBeams) {
            StartCoroutine(buildBeams());
        }
        StartCoroutine(buildCases()); //S'occupe de la création des cubes de destruction qui gère une case

        StartCoroutine(buildMaze());

        //Pour exécution après la fin de génération de la map voir void whenGenerated
        
    }

    void whenGenerated() //Fonction s'éxecutant à la fin de la génération
    {
        spawnPlayer();
        spawnTorch();

        Destroy(menuCapsule); // Nécessaire pour ne pas avoir plusieurs levelSelector si le joueur joue plusieurs parties
    }


    //NB: X correspond à l'axe X du transform et Y à l'axe Z du transform


    void getLevelInfos()
    {
        menuCapsule = GameObject.Find("menuCapsule").GetComponent<menu_capsule>();

        playerName = menuCapsule.playerName;

        string lvl = menuCapsule.difficulty; // lvl = difficulty
        switch (lvl)
        {
            case "easy":
                mazeSize = 10;
                break;
            case "medium":
                mazeSize = 15;
                break;
            case "expert":
                mazeSize = 20;
                break;
            case "supereasy":
                mazeSize = 4;
                break;
        }
    }


    void spawnTorch() // Spawn une lamp grabbable près du joueur
    {
        GameObject spawnedTorch = Instantiate(torchGO);
        GameObject case1 = GameObject.Find("cell1");
        spawnedTorch.transform.position = case1.transform.position + new Vector3(0,0.1f,0);
        spawnedTorch.transform.rotation = Quaternion.identity;
    }

    void buildGround(){
		GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
		ground.transform.name = "ground";
		ground.transform.localScale= new Vector3(mazeSize*caseSize*0.1f,1,mazeSize*caseSize*0.1f);
        ground.GetComponent<Renderer>().material = matGround;
        ground.transform.parent = transform;

    }

    void spawnPlayer()
    {
        int[] caze = { 0, 0 };
        GameObject ply = Instantiate(player);
        ply.transform.position = caseToVect3(caze);
        ply.transform.name = playerName;
    }

	void buildWalls(){
		
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //GameObject entry;
        //GameObject exit;
        GameObject hWalls = new GameObject();
        hWalls.transform.name = "hWalls";
		wall.transform.localScale = new Vector3 (caseSize, heightWall, wallThickeness);
        wall.transform.position = new Vector3(500, 0, 0); //on écart le mur de base pour ne pas le voir durant la construction du labyrinthe
		Renderer rend = wall.GetComponent<Renderer> ();
		rend.material = matWalls;

		for(int X =  0; X < mazeSize; X++){

			for (int Y = 0; Y < mazeSize + 1; Y++) {

				GameObject wallX = Instantiate (wall);
				wallX.transform.name = "wall" + X.ToString () + Y.ToString ();
				wallX.transform.position = new Vector3 (X * caseSize - center[0] + caseSize*0.5f, heightWall/2, Y*caseSize - center[1]);
                wallX.transform.parent = hWalls.transform;

            }
        }
        GameObject vWalls = Instantiate(hWalls);

        vWalls.transform.parent = transform;
        vWalls.transform.Rotate(0,90,0);
        vWalls.transform.name = "vWalls";

        Destroy(wall); //on détruit le modèle du mur construit pour l'instantiate de chaque mur

        //-- On met tous les murs dans un GO
        GameObject wallsGrp = new GameObject();
        wallsGrp.transform.name = "walls";
        wallsGrp.transform.parent = transform;
        vWalls.transform.parent = wallsGrp.transform;
        hWalls.transform.parent = wallsGrp.transform;
        //--

        //Construction du plafond
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ceiling.transform.name = "ceiling";
        ceiling.transform.localScale = new Vector3(caseSize * mazeSize * 0.1f, 1, caseSize * mazeSize * 0.1f);
        ceiling.transform.Rotate(0,0,180);
        ceiling.transform.position = Vector3.zero + new Vector3(0,heightWall,0);
        ceiling.GetComponent<Collider>().enabled = false;
        ceiling.GetComponent<Renderer>().material = matCeiling;
        ceiling.transform.parent = transform;
    }

    IEnumerator buildBeams()
    {

        GameObject beams = new GameObject();
        beams.transform.name = "beams";
        beams.transform.parent = transform;

        GameObject beamGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        beamGO.transform.localScale = new Vector3(wallThickeness * beamThickness, heightWall,wallThickeness * beamThickness);
        beamGO.transform.name = "beam0";
        beamGO.GetComponent<Renderer>().material = matBeams;
        //print("beam-" + Mathf.FloorToInt(mazeSize / 2));

        Vector3 vectDec = new Vector3(-caseSize/2,0, -caseSize / 2); //Si on instantie les beam au niveau de chaque case, elles seront au centre, il faut donc les décaler

        GameObject beamActive; //Poutre en cours de placement (évite de recréer la variable à chaque fois)

        for (int X = 0; X < mazeSize+1; X++)
        {
            for (int Y = 0; Y < mazeSize+1; Y++)
            {
                
                int[] cazze = {0,0};
                cazze[0] = X;
                cazze[1] = Y;
                beamActive = Instantiate(beamGO, caseToVect3(cazze) + vectDec, Quaternion.Euler(0,0,0));
                beamActive.transform.name = "beam" + X.ToString() + Y.ToString();
                beamActive.transform.parent = beams.transform;
                //print("beam" + X + Y);
                
            }
            yield return null;

        }

        Destroy(beamGO);
        

    }

    IEnumerator buildCases()
    {
        //yield return new WaitForSeconds(1f);
        etatCase = new bool[mazeSize, mazeSize];
        cubeCase = new GameObject[mazeSize, mazeSize];
        GameObject cubeC = GameObject.CreatePrimitive(PrimitiveType.Cube); //cube Case
        cubeC.transform.localScale = new Vector3(caseSize*0.2f, heightWall * 0.2f, caseSize*0.2f);
        cubeC.GetComponent<Collider>().enabled = false;
        cubeC.isStatic = true;
        Renderer rend = cubeC.GetComponent<Renderer>();
        //rend.material.color = Color.green;
        rend.enabled = false;
        for (int X = 0; X < mazeSize; X++)
        {
            for (int Y = 0; Y < mazeSize; Y++)
            {
                GameObject cube = Instantiate(cubeC);
                cube.transform.tag = "cell";
                cube.transform.name = "cell" + (X*mazeSize + Y).ToString();
                cube.transform.position = new Vector3(X * caseSize - center[0] + caseSize*0.5f, heightWall * 0.1f, Y * caseSize - center[1] + caseSize * 0.5f);
                cube.transform.parent = casesGO.transform;
                cubeCase[X, Y] = cube;
                etatCase[X, Y] = false;
            }
            //yield return null;
        }
        Destroy(cubeC);
        yield return null;
        
    }

    void UpdateCases(int[] caze, bool etat)
    {
        etatCase[caze[0], caze[1]] = etat;
        //print(caze[0] + caze[1]);
        if (etat) {
            cubeCase[caze[0], caze[1]].GetComponent<Renderer>().material.color = Color.red;
            cubeCase[caze[0], caze[1]].transform.name = "cell" + compteur.ToString();
        }
        else
            cubeCase[caze[0], caze[1]].GetComponent<Renderer>().material.color = Color.green;
    }

    int[,] voisinsLibre(int[] caze) //cf fin de la fonction  --  nombres de voisins libres = (voisinsLibre(CASE).Length/2
    {
        int[,] voisinsLibres= new int[4,2];
        int nbVoisins = 0;
        int X_c = caze[0];
        int Y_c = caze[1];
        //à droite
        try
        {
            if (!etatCase[X_c+1, Y_c])
            {
                voisinsLibres[nbVoisins,0] = X_c + 1;
                voisinsLibres[nbVoisins,1] = Y_c;
                nbVoisins += 1;
            }
        }
        catch{
            nbVoisins += 0;
        }
        //à gauche
        try
        {
            if (!etatCase[X_c - 1, Y_c])
            {
                voisinsLibres[nbVoisins, 0] = X_c - 1;
                voisinsLibres[nbVoisins, 1] = Y_c;
                nbVoisins += 1;
            }
        }
        catch
        {
            nbVoisins += 0;
        }
        //en haut
        try
        {
            if (!etatCase[X_c, Y_c - 1])
            {
                voisinsLibres[nbVoisins, 0] = X_c;
                voisinsLibres[nbVoisins, 1] = Y_c - 1;
                nbVoisins += 1;
            }
        }
        catch
        {
            nbVoisins += 0;
        }
        //en bas
        try
        {
            if (!etatCase[X_c, Y_c + 1])
            {
                voisinsLibres[nbVoisins, 0] = X_c;
                voisinsLibres[nbVoisins, 1] = Y_c + 1;
                nbVoisins += 1;
            }
        }
        catch
        {
            nbVoisins += 0;
        }
        int[,] returnList = new int[nbVoisins,2]; //Première colonne : Remplie du nombre de voisins -- C'est la liste voisinsLibres mais de taille nbVoisins
        for (int i = 0; i < nbVoisins; i++)
        {
            returnList[i, 0] = voisinsLibres[i, 0];
            returnList[i, 1] = voisinsLibres[i, 1];
        }
        return returnList;
    }

    IEnumerator buildMaze()
    {
        //yield return new WaitForSeconds(2f);
        yield return null;
        int[] caseInit = { 0, 0 };
        UpdateCases(caseInit, true);
        int [] caseChoisie = caseInit;
        int[,] fakeQueue = new int[mazeSize*mazeSize, 2]; //Une liste qui sera utilisée comme queue LIFO , si pas d'élément : {-1,-1}
        fakeQueue[0, 0] = caseInit[0];
        fakeQueue[0, 1] = caseInit[1];
        fakeQueue[1, 0] = -1;//Element libre
        fakeQueue[1, 1] = -1;
        compteur = 1;
        while (QueueLength(fakeQueue) >= 1){ 
            while (voisinsLibre(caseChoisie).Length > 0)
            {
                int [] newCaseChoisie = chooseCaseLibre(caseChoisie);
                destroyWallBtwn(caseChoisie, newCaseChoisie); 
                caseChoisie = newCaseChoisie;
                UpdateCases(caseChoisie, true);
                fakeQueue = addToQueue(caseChoisie, fakeQueue);
                compteur += 1;
                //yield return null;
                if ((compteur % 1) == 0){
                    //yield return null;
                }
            }
            deQueue(fakeQueue);
            int[] caseC = { 0, 0 };
            if (QueueLength(fakeQueue) != 0) //condition de fin de la génération :)
            {
                caseC[0] = fakeQueue[QueueLength(fakeQueue) - 1, 0];
                caseC[1] = fakeQueue[QueueLength(fakeQueue) - 1, 1];//pour la sortir de la queue + opti
            }
            //print("Fin de boucle , case choisie : X" + caseC[0] + "Y" + caseC[1]);
            //yield return null;
            caseChoisie = caseC;
        }
        print("GENERATION FINIE");
        isGenerated = true;

        whenGenerated();

        //casesGO.SendMessage("getCellsType");
        Destroy(casesGO); // Supprime l'ensemble des cases à présent inutiles

    }

    int[] chooseCaseLibre(int[] caze)
    {
        int[] caseChoisie = { 0 , 0 };
        int[,] voisinsLib = voisinsLibre(caze);
        int nbV = voisinsLib.Length / 2;    //nb de voisins libres
        //print("Nombre de voisins libres : " + nbV);
        int voisinChoisi = Random.Range(0, nbV);
        //print("Voisin choisi :" + voisinChoisi);
        caseChoisie[0] = voisinsLib[voisinChoisi, 0];
        caseChoisie[1] = voisinsLib[voisinChoisi, 1];
        return caseChoisie;
    }

    void destroyWallBtwn(int[] case1, int[] case2)
    {
        Vector3 C1ToC2 = new Vector3(case2[0] - case1[0], 0 , case2[1] - case1[1]); //vecteur unitaire de direction 
        
        RaycastHit hit;
        if (Physics.Raycast(caseToVect3(case1), C1ToC2, out hit, caseSize))
        {
            //Debug.DrawRay(caseToVect3(case1), C1ToC2 * hit.distance, Color.magenta, 2, true);
            Destroy(hit.transform.gameObject);
        }
        else
        {
            print("Le RayCast n'as pas percuté de mur !");
        }
        //Debug.DrawLine(caseToVect3(case1), caseToVect3(case2), Color.red, 40f, false);
    }

    void buildDoor(Vector3 posEnt)
    {
        Instantiate(doorGO, posEnt + new Vector3(-caseSize/2,0,0), Quaternion.identity);
    }

    void entryAndExit() //Fonctionne pour l'instant que si entrée en haut et sortie en bas
    {
        int[] entryCell = { 0, 0 };
        int[] exitCell = { mazeSize - 1, mazeSize - 1 };
        Vector3 posEntry = caseToVect3(entryCell);
        Vector3 posExit = caseToVect3(exitCell);

        buildDoor(posEntry); //Fonctionne que si on pose l'entrée en haut (eq. à dire que entryCell[0] = 0)

        /*Debug.DrawRay(posEntry, new Vector3(-1,0,0)*10, Color.red, 50, true);
        Debug.DrawRay(posExit, new Vector3(1, 0, 0) * 10, Color.red, 50, true);*/

        RaycastHit hit1;
        RaycastHit hit2;

        if (Physics.Raycast(posEntry, new Vector3(-1, 0, 0), out hit1, caseSize))
        {
            Destroy(hit1.transform.gameObject);
        }

        if (Physics.Raycast(posExit, new Vector3(1, 0, 0), out hit2, caseSize))
        {
            hit2.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            hit2.transform.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            hit2.transform.gameObject.AddComponent<exit_script>();
            hit2.transform.name = "exit";
        }

    }

    Vector3 caseToVect3(int[] caze)
    {
        float _XC0 = (caseSize / 2 * (1 - mazeSize)); //calcul fait au tableau
        int XC0 = (int) _XC0; //XC0 = YC0 !
        //print("XC0 ! " + XC0);
        int[] coordCase = { XC0 + caseSize * caze[0] , XC0 + caseSize * caze[1] }; //peut être opti +
        Vector3 returnVect = new Vector3(coordCase[0], heightWall*0.5f, coordCase[1]);
        return returnVect;
    }

    int QueueLength(int[,] Q)
    {
        for(int i = 0; i< mazeSize * mazeSize; i++)
        {
            if (Q[i,0] == - 1 && Q[i, 1] == -1)
            {
                return i;
            }
        }
        return mazeSize;
    }

    int[,] addToQueue(int[] caze, int[,] Q)
    {
        int[,] Q1 = Q;
        int lengthQ = QueueLength(Q);
        //print(lengthQ);
        Q1[lengthQ, 0] = caze[0];
        Q1[lengthQ, 1] = caze[1];
        Q1[lengthQ+1, 0] = -1;
        Q1[lengthQ+1, 1] = -1;
        //print("Ajout de la case X" + caze[0] + "Y" + caze[1] + ", taille queue :" + QueueLength(Q1));
        return Q1;
    }

    int[,] deQueue(int[,] Q)
    {
        int[,] Q1 = Q;
        int lengthQ = QueueLength(Q);
        Q1[lengthQ-1, 0] = -1;
        Q1[lengthQ-1, 1] = -1;
        //print("Dequeue, taille queue : " + QueueLength(Q1));
        return Q1;
    }
}
