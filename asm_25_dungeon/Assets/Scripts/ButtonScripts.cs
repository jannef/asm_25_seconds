using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScripts : MonoBehaviour {
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);
    }
    public void OpenFeedback()
    {
        Application.OpenURL("https://goo.gl/forms/fw9f0rKWmRZiKt0s1");
    }
}
