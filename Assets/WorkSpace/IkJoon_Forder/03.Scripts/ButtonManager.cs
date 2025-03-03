using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonManager : MonoBehaviour
{
    public GameObject slotMachine;
    private PotalController _potal;
    
    public void OnClickStartBtn()
    {
        SceneManager.LoadScene("Stage");
    }
    public void OnClickExitBtn()
    {
         #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
    public void OnClickUpgradeBtn()
    {
        slotMachine.SetActive(false);
    }
}
