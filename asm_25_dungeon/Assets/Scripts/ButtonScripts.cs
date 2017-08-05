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
}
