using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ambience_selector : MonoBehaviour
{

    private Vector4[] ambience1 = new Vector4[] { new Vector4(72, 0, 29), new Vector4(63, 0, 10), new Vector4(48, 0, 41) }; // w, x, y : Sky color, Equator color, Ground color
    private Vector4[] ambience2 = new Vector4[] { new Vector4(0, 11, 111), new Vector4(34, 0, 115), new Vector4(87, 0, 65) };
    private Vector4[] ambience3 = new Vector4[] { new Vector4(76, 0, 111), new Vector4(67, 3, 140), new Vector4(87, 30, 0) }; 
    private Vector4[] ambience4 = new Vector4[] { new Vector4(0, 1, 77), new Vector4(11, 3, 63), new Vector4(20, 26, 79) };

    //private Vector3[][] ambiences = new Vector3[][] {ambience1,ambience2}; 

    void Start()
    {
        int randomNumber = Random.Range(0, 4);
        print("Choix de l'ambience" + (randomNumber+1).ToString());

        Vector4[] ambienceChoisi = ambience1;

        switch (randomNumber)
        {
            case 0:
                ambienceChoisi = ambience1;
                break;
            case 1:
                ambienceChoisi = ambience2;
                break;
            case 2:
                ambienceChoisi = ambience3;
                break;
            case 3:
                ambienceChoisi = ambience4;
                break;
        }

        RenderSettings.ambientMode = AmbientMode.Trilight;

        RenderSettings.ambientSkyColor = convertVect4_255toVect4_Float1(ambienceChoisi[0]);
        RenderSettings.ambientEquatorColor = convertVect4_255toVect4_Float1(ambienceChoisi[1]);
        RenderSettings.ambientGroundColor = convertVect4_255toVect4_Float1(ambienceChoisi[2]);

    }

    Vector4 convertVect4_255toVect4_Float1(Vector3 color255) // Convertit (255,0,128) en (1,0,0.5f,0)
    {
        Vector4 newColor = new Vector4(0, 0, 0);
        newColor.x = (color255.x * 1.0f) / 255;
        newColor.y = (color255.y * 1.0f) / 255;
        newColor.z = (color255.z * 1.0f) / 255;
        return newColor;
    }


}
