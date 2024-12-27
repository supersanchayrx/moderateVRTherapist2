using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadHomeScene : MonoBehaviour
{
    public void LoadScene(int sceneIndexToLoad)
    {
        Debug.Log($"Loading Scene with Index: {sceneIndexToLoad}");

        // Load the target scene (Scene Index 0)
        SceneManager.LoadScene(sceneIndexToLoad, LoadSceneMode.Single);
    }
}
