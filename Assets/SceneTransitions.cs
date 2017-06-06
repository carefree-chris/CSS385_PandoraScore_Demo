using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;


public class SceneTransitions : MonoBehaviour {

    /*
    [YarnCommand("print")]
    public void Print(string words)
    {
        Debug.Log("WORDS: " + words);
    }

    [YarnCommand("SceneChange")]
    public void TransitionToScene(string sceneName)
    {
        Debug.Log("Transitioning to Next Scene: " + sceneName);

        SceneManager.LoadScene(sceneName);
    }*/

    #region Yarn Commands
    [YarnCommand("SceneChange")]
    public void TransitionToScene(string sceneName)
    {
        Debug.Log("Transitioning to Next Scene: " + sceneName);

        SceneManager.LoadScene(sceneName);
    }

    #endregion
}
