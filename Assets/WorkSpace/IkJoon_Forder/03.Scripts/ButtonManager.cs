using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonManager : MonoBehaviour
{
    public string sceneName;
    
    public void OnClickStartBtn()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void OnClickExitBtn()
    {
         #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
