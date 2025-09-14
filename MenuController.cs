using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnPlayClick()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void OnQuitClick()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
