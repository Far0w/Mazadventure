using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    private GameObject cube;
    private Vector3 posCible; //La position de la cible
    public GameObject cibleGO;

    void Start()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube); //A remplacer par Instnatiate pur mettre une vraie cible
        cube.transform.localScale = 0.2f * Vector3.one;
        cube.transform.GetComponent<Renderer>().material.color = Color.magenta;
        cube.transform.GetComponent<Collider>().enabled = false; //on désactive pour ne pas gêner le Raycast
        //StartCoroutine(reloadRay());
    }

    void Update()
    {

        superFonction(transform.up);

    }

    void superFonction(Vector3 vectZero)
    {
        float sizeVect = 0.6f; // plus grand plus opti
        Vector3 vectInterm;
        Vector3 posInterm;
        bool foundIntersection = false; //le raycast a rencontré le sol ?
        int compteur = 0;
        Vector3 posInters = Vector3.zero; //postion intersection
        Vector3 intersToPlayer; //vecteur du joueur to Intersection

        vectInterm = vectZero * sizeVect; //vecteur de direction
        posInterm = transform.position; //Initialisation de la position de départ
        Debug.DrawRay(posInterm, vectInterm, Color.cyan, 0.04f, false);
        posInterm = vectInterm + posInterm;
        cube.transform.position = Vector3.Lerp(cube.transform.position, posCible, Time.deltaTime*10);
        while (compteur < 18 && foundIntersection == false)
        {
            vectInterm = Quaternion.Euler(25 * sizeVect, 0, 0) * vectInterm;
            Debug.DrawRay(posInterm, vectInterm, Color.cyan, 0.04f, true);

            RaycastHit rHit;

            if (Physics.Raycast(posInterm, vectInterm, out rHit, sizeVect))
            {
                if (rHit.transform.name == "Ground") {
                    posInters = rHit.point;
                    posCible = posInters;
                
                    foundIntersection = true;

            }
            }
            else
            {
                posInters = vectZero; //Si  aucune intersection : interToPlayer = Vecteur nul
            }
            intersToPlayer = transform.position - posInters;
            //Debug.DrawLine(vectZero, posInters, Color.blue, 0.02f);
            compteur += 1;
            //printVector3 (posInterm , "posInterm");
            posInterm = vectInterm + posInterm;
        }


    }

    /*IEnumerator reloadRay()
    {
        for (int i = 0; i < 50000; i++)
        {
            superFonction(transform.up);
            yield return new WaitForSeconds(0.04f);
        }


    }*/

    void printVector3(Vector3 v, string txt)
    {
        print(txt + "X" + v.x + "Y" + v.y + "Z" + v.z);
    }

}
