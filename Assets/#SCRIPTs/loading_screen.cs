using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loading_screen : MonoBehaviour
{
    
    public Image progressBar;

    void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        yield return new WaitForSeconds(2f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/game");

        while (asyncLoad.progress < 1)
        {
            progressBar.fillAmount = asyncLoad.progress;
            yield return null;
        }

        yield return null;
    }
}
