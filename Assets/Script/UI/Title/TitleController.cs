using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
   
{
    public void GoNextScene()
    {
        LoadSceneManager.Instance.LoadSceneAsync(LoadSceneManager.SceneState.Game);
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect((Screen.width - 150) / 2, (Screen.height - 50) / 2, 150, 50), "START"))
        {
            GoNextScene();
        }
    }
}
