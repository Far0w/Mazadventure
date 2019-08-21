using UnityEngine;
using System.Collections;

public class cells_behavior : MonoBehaviour {

    private GameObject[] cells;

	void getCellsType() 
    //Différencie 4 type de cellules par nombres de murs et organisation => 
    //1 : couloir 1 mur (sur les bords)
    //2 : couloir 2 murs 
    //3 : corner (2 murs mais collés)
    //4 : end (3 murs)
    {
        cells = GameObject.FindGameObjectsWithTag("cell");
        for (int i = 0; i< cells.Length; i++)
        {
            //print(i);
        }
    }

    int getCellType(GameObject cell)
    {
        cell.GetComponent < Renderer > ().enabled = true;
        return 1;
    }
}
